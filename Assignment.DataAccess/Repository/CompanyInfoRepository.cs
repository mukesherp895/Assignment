using Assignment.Common;
using Assignment.DataAccess.DBContext;
using Assignment.Model.Domain;
using Assignment.Model.DTO;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace Assignment.DataAccess.Repository
{
    public interface ICompanyInfoRepository
    {
        Task<EnumData.DBAttempt> AddAsync(CompanyInfos companyInfos);
        Task<EnumData.DBAttempt> DeleteAsync(CompanyInfos companyInfos);
        Task<List<CompanyInfosDto>> GetAllCompanyByFilterAsync(DataTableParamDto dto);
        Task<List<DropDownListDto>> GetAllCompanyBySearchAsync(string search);
        Task<CompanyInfos> GetByIdCompanyInfoAsync(int id);
        Task<EnumData.DBAttempt> UpdateAsync(CompanyInfos companyInfos);
    }

    public class CompanyInfoRepository : ICompanyInfoRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public CompanyInfoRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<EnumData.DBAttempt> AddAsync(CompanyInfos companyInfos)
        {
            //Create Schema
            string schema = string.Empty;
            if(!await _dbContext.CompanyInfos.AnyAsync()) 
            {
                schema = "C1";
            }
            else
            {
                schema = "C" + (_dbContext.CompanyInfos.Max(m => m.Id) + 1).ToString();
            }
            companyInfos.Schema = schema;

            _dbContext.CompanyInfos.Add(companyInfos);
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                var param = new SqlParameter[]
                {
                    new SqlParameter("@productSchema", companyInfos.Schema)
                };
                await _dbContext.Database.ExecuteSqlRawAsync("EXEC [dbo].[Sp_CreateSchemaTableCompanyWise] @productSchema", param);
                return EnumData.DBAttempt.Success;
            }
            return EnumData.DBAttempt.Fail;
        }
        public async Task<EnumData.DBAttempt> UpdateAsync(CompanyInfos companyInfos)
        {
            _dbContext.CompanyInfos.Update(companyInfos);
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return EnumData.DBAttempt.Success;
            }
            return EnumData.DBAttempt.Fail;
        }
        public async Task<EnumData.DBAttempt> DeleteAsync(CompanyInfos companyInfos)
        {
            //_dbContext.Entry(companyInfos).State = EntityState.Deleted;
            _dbContext.Remove(companyInfos);
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                string sqlQuery = $"DROP TABLE [{companyInfos.Schema}].[Product]";
                await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery);
                return EnumData.DBAttempt.Success;
            }
            return EnumData.DBAttempt.Fail;
        }
        public async Task<List<CompanyInfosDto>> GetAllCompanyByFilterAsync(DataTableParamDto dto)
        {
            using (SqlConnection conn = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                var parameters = new { displayStart = dto.iDisplayStart, displayLength = dto.iDisplayLength, sortDir = dto.sSortDir_0 ?? "asc", sortCol = dto.iSortCol_0, search = dto.sSearch ?? "" };
                await conn.OpenAsync();
                var data = await conn.QueryAsync<CompanyInfosDto>("Sp_GetCompanyInfoByFilter", parameters, commandType: System.Data.CommandType.StoredProcedure);
                return data.ToList();
            }
        }
        public async Task<CompanyInfos> GetByIdCompanyInfoAsync(int id)
        {
            return await _dbContext.CompanyInfos.Where(w => w.Id == id).FirstOrDefaultAsync() ?? new CompanyInfos();
        }
        public async Task<List<DropDownListDto>> GetAllCompanyBySearchAsync(string search)
        {
            return await _dbContext.CompanyInfos.Where(w => w.CompanyName.Contains(search)).Select(s => new DropDownListDto { Id = s.Id, Name = s.CompanyName, Schema = s.Schema }).ToListAsync();
        }
    }
}

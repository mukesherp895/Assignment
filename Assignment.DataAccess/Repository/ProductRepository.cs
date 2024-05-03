using Assignment.DataAccess.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Assignment.Model.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Assignment.Common;
using System.Net.Http.Headers;
using Assignment.Model.Domain;

namespace Assignment.DataAccess.Repository
{
    public interface IProductRepository
    {
        Task<EnumData.DBAttempt> AddAsync(ProductReqDto productReqDto, string userName);
        Task<EnumData.DBAttempt> DeleteAsync(int productId, int companyInfoId);
        Task<List<ProductsDto>> GetAllProductAsync(string? schema);
        Task<EnumData.DBAttempt> UpdateAsync(int productId, ProductReqDto productReqDto, string userName);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public ProductRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ProductsDto>> GetAllProductAsync(string? schema)
        {
            using (SqlConnection conn = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                var parameters = new { schema = schema ?? "" };
                await conn.OpenAsync();
                var data = await conn.QueryAsync<ProductsDto>("Sp_ProductGet", parameters, commandType: System.Data.CommandType.StoredProcedure);
                return data.ToList();
            }
        }
        public async Task<EnumData.DBAttempt> AddAsync(ProductReqDto productReqDto, string userName)
        {
            var companyInfo = await _dbContext.CompanyInfos.Where(w => w.Id == productReqDto.CompanyInfoId).FirstOrDefaultAsync();
            if (companyInfo != null) 
            {
                var param = new SqlParameter[]
                {
                    new SqlParameter("@title", productReqDto.Title),
                    new SqlParameter("@description", productReqDto.Description),
                    new SqlParameter("@price", productReqDto.Price),
                    new SqlParameter("@createdBy", userName),
                    new SqlParameter("@createdDateTime", DateTime.Now)
                };
                string sqlQuery = $"INSERT INTO [{companyInfo.Schema}].[Product] ([Title],[Description],[Price],[CreatedBy],[CreatedDateTime]) VALUES (@title,@description,@price,@createdBy,@createdDateTime)";
                if(await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery, param) > 0)
                {
                    return EnumData.DBAttempt.Success;
                }
            }
            return EnumData.DBAttempt.Fail;
        }
        public async Task<EnumData.DBAttempt> UpdateAsync(int productId, ProductReqDto productReqDto, string userName)
        {
            var companyInfo = await _dbContext.CompanyInfos.Where(w => w.Id == productReqDto.CompanyInfoId).FirstOrDefaultAsync();
            if (companyInfo != null)
            {
                var param = new SqlParameter[]
                {
                    new SqlParameter("@title", productReqDto.Title),
                    new SqlParameter("@description", productReqDto.Description),
                    new SqlParameter("@price", productReqDto.Price),
                    new SqlParameter("@updatedBy", userName),
                    new SqlParameter("@updatedDateTime", DateTime.Now),
                    new SqlParameter("@productId", productId)
                };
                string sqlQuery = $"UPDATE [{companyInfo.Schema}].[Product] SET [Title]=@title,[Description]=@description,[Price]=@price,[UpdatedBy]=@updatedBy,[UpdatedDateTime]=@updatedDateTime WHERE Id=@productId";
                if (await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery, param) > 0)
                {
                    return EnumData.DBAttempt.Success;
                }
            }
            return EnumData.DBAttempt.Fail;
        }
        public async Task<EnumData.DBAttempt> DeleteAsync(int productId, int companyInfoId)
        {
            var companyInfo = await _dbContext.CompanyInfos.Where(w => w.Id == companyInfoId).FirstOrDefaultAsync();
            if (companyInfo != null)
            {
                var param = new SqlParameter[]
                {
                    new SqlParameter("@productId", productId)
                };
                string sqlQuery = $"DELETE FROM [{companyInfo.Schema}].[Product] WHERE Id=@productId";
                if (await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery, param) > 0)
                {
                    return EnumData.DBAttempt.Success;
                }
            }
            return EnumData.DBAttempt.Fail;
        }
    }
}

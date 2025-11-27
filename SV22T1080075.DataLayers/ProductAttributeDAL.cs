using Dapper;
using SV22T1080075.DomainModels;
using System.Data;

namespace SV22T1080075.DataLayers
{
    public class ProductAttributeDAL : BaseDAL
    {
        public ProductAttributeDAL(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Danh sách thuộc tính của 1 mặt hàng
        /// </summary>
        public async Task<IEnumerable<ProductAttribute>> ListAsync(int productId)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT * FROM ProductAttributes
                            WHERE ProductID = @productId
                            ORDER BY DisplayOrder";

                return await connection.QueryAsync<ProductAttribute>(
                    sql, new { productId }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Lấy 1 thuộc tính theo ID
        /// </summary>
        public async Task<ProductAttribute?> GetAsync(long attributeId)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT * FROM ProductAttributes WHERE AttributeID = @attributeId";

                return await connection.QueryFirstOrDefaultAsync<ProductAttribute>(
                    sql, new { attributeId }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Thêm thuộc tính
        /// </summary>
        public async Task<long> AddAsync(ProductAttribute data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"
                    INSERT INTO ProductAttributes(ProductID, AttributeName, AttributeValue, DisplayOrder)
                    VALUES (@ProductID, @AttributeName, @AttributeValue, @DisplayOrder);
                    SELECT SCOPE_IDENTITY();";

                return await connection.ExecuteScalarAsync<long>(
                    sql, data, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Cập nhật thuộc tính
        /// </summary>
        public async Task<bool> UpdateAsync(ProductAttribute data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"
                    UPDATE ProductAttributes
                    SET AttributeName = @AttributeName,
                        AttributeValue = @AttributeValue,
                        DisplayOrder = @DisplayOrder
                    WHERE AttributeID = @AttributeID";

                return await connection.ExecuteAsync(
                    sql, data, commandType: CommandType.Text) > 0;
            }
        }

        /// <summary>
        /// Xóa thuộc tính
        /// </summary>
        public async Task<bool> DeleteAsync(long attributeId)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"DELETE FROM ProductAttributes WHERE AttributeID = @attributeId";

                return await connection.ExecuteAsync(
                    sql, new { attributeId }, commandType: CommandType.Text) > 0;
            }
        }
    }
}

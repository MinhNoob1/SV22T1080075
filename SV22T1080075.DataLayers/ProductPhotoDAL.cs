using Dapper;
using SV22T1080075.DomainModels;
using System.Data;

namespace SV22T1080075.DataLayers
{
    public class ProductPhotoDAL : BaseDAL
    {
        public ProductPhotoDAL(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Lấy danh sách ảnh của 1 mặt hàng
        /// </summary>
        public async Task<IEnumerable<ProductPhoto>> ListAsync(int productId)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT * FROM ProductPhotos
                            WHERE ProductID = @productId
                            ORDER BY DisplayOrder";

                return await connection.QueryAsync<ProductPhoto>(
                    sql, new { productId }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Lấy 1 ảnh theo ID
        /// </summary>
        public async Task<ProductPhoto?> GetAsync(long photoId)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT * FROM ProductPhotos WHERE PhotoID = @photoId";

                return await connection.QueryFirstOrDefaultAsync<ProductPhoto>(
                    sql, new { photoId }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Thêm ảnh mới
        /// </summary>
        public async Task<long> AddAsync(ProductPhoto data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"
                    INSERT INTO ProductPhotos(ProductID, Photo, Description, DisplayOrder, IsHidden)
                    VALUES (@ProductID, @Photo, @Description, @DisplayOrder, @IsHidden);
                    SELECT SCOPE_IDENTITY();";

                return await connection.ExecuteScalarAsync<long>(
                    sql, data, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Cập nhật ảnh
        /// </summary>
        public async Task<bool> UpdateAsync(ProductPhoto data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"
                    UPDATE ProductPhotos
                    SET Photo = @Photo,
                        Description = @Description,
                        DisplayOrder = @DisplayOrder,
                        IsHidden = @IsHidden
                    WHERE PhotoID = @PhotoID";

                return await connection.ExecuteAsync(sql, data, commandType: CommandType.Text) > 0;
            }
        }

        /// <summary>
        /// Xóa ảnh
        /// </summary>
        public async Task<bool> DeleteAsync(long photoId)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"DELETE FROM ProductPhotos WHERE PhotoID = @photoId";

                return await connection.ExecuteAsync(
                    sql, new { photoId }, commandType: CommandType.Text) > 0;
            }
        }
    }
}

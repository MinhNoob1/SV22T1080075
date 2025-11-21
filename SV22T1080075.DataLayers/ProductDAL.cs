using Dapper;
using SV22T1080075.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV22T1080075.DataLayers
{
    /// <summary>
    /// Cung cấp các chức năng xử lý dữ liệu liên quan đến sản phẩm (Product)
    /// </summary>
    public class ProductDAL : BaseDAL
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public ProductDAL(string connectionString) : base(connectionString) {}
        /// <summary>
        /// Tìm kiếm và lấy danh sách mặt hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng mỗi trang (0 thì không phân trang)</param>
        /// <param name="searchValue">Giá trị tìm kiếm (chuỗi rỗng nếu lấy toàn bộ)</param>
        /// <param name="categoryID">Mã loại hàng cần tìm (0 nếu lấy tất cả loại hàng)</param>
        /// <param name="supplierID">Mã nhà cung cấp (0 nếu lấy của tất cả nhà cung cấp)</param>
        /// <param name="minPrice">Giá nhỏ nhất (0 nếu không hạn chế nhỏ nhất)</param>
        /// <param name="maxPrice">Giá lớn nhất (0 nếu không hạn chế giá lớn nhất)</param>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> ListAsync(int page = 1, int pageSize = 0,
                                    string searchValue = "", int categoryID = 0, int supplierID = 0,
                                    decimal minPrice = 0, decimal maxPrice = 0)
        {
            if (page < 1) page = 1;
            if (pageSize < 0) pageSize = 0;
            searchValue = $"%{searchValue}%";

            using var connection = await OpenConnectionAsync();
            var sql = @"with cte as
                        (
                            select  *,
                                    row_number() over(order by ProductName) as RowNumber
                            from    Products 
                            where   (ProductName like @SearchValue)
                                and (@CategoryID = 0 or CategoryID = @CategoryID)
                                and (@SupplierID = 0 or SupplierId = @SupplierID)
                                and (Price >= @MinPrice)
                                and (@MaxPrice <= 0 or Price <= @MaxPrice)
                        )
                        select * from cte 
                        where   (@PageSize = 0) 
                            or (RowNumber between (@Page - 1)*@PageSize + 1 and @Page * @PageSize)";
            var parameters = new
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue ?? "",
                CategoryID = categoryID,
                SupplierID = supplierID,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };
            return await connection.QueryAsync<Product>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="categoryID"></param>
        /// <param name="supplierID"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        public async Task<int> CountAsync(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            searchValue = $"%{searchValue}%";
            using var connection = await OpenConnectionAsync();
            var sql = @"select  count(*)
                            from    Products 
                            where   (@SearchValue = N'' or ProductName like @SearchValue)
                                and (@CategoryID = 0 or CategoryID = @CategoryID)
                                and (@SupplierID = 0 or SupplierId = @SupplierID)
                                and (Price >= @MinPrice)
                                and (@MaxPrice <= 0 or Price <= @MaxPrice)";
            var parameters = new
            {
                SearchValue = searchValue ?? "",
                CategoryID = categoryID,
                SupplierID = supplierID,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };
            return await connection.ExecuteScalarAsync<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
        }
        /// <summary>
        /// Lấy thông tin chi tiết của một sản phẩm
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Product?> GetAsync(int id)
        {
            using var connection = await OpenConnectionAsync();
            var sql = @"SELECT * FROM Products WHERE ProductId = @id";
            var parameters = new
            {
                id
            };
            return await connection.QueryFirstOrDefaultAsync<Product>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
        }
        /// <summary>
        /// Thêm một sản phẩm mới
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> AddAsync(Product data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"INSERT INTO Products
                            (
                                ProductName,
                                ProductDescription,
                                SupplierID,
                                CategoryID,
                                Unit,
                                Price,
                                Photo,
                                IsSelling
                            )
                            VALUES
                            (
                                @ProductName,
                                @ProductDescription,
                                @SupplierID,
                                @CategoryID,
                                @Unit,
                                @Price,
                                @Photo,
                                @IsSelling
                            );
                            SELECT SCOPE_IDENTITY();";

                return await connection.ExecuteScalarAsync<int>(sql: sql, param: data, commandType: CommandType.Text);
            }
        }
        /// <summary>
        /// Cập nhật thông tin sản phẩm
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateAsync(Product data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"UPDATE Products
                            SET     ProductName = @ProductName,
                                    ProductDescription = @ProductDescription,
                                    SupplierID = @SupplierID,
                                    CategoryID = @CategoryID,
                                    Unit = @Unit,
                                    Price = @Price,
                                    Photo = @Photo,
                                    IsSelling = @IsSelling
                            WHERE   ProductID = @ProductID;";

                return (await connection.ExecuteAsync(sql: sql, param: data, commandType: CommandType.Text)) > 0;
            }
        }
        /// <summary>
        /// Xóa một sản phẩm
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"DELETE FROM Products WHERE ProductID = @id;";
                return (await connection.ExecuteAsync(sql: sql, param: new { id }, commandType: CommandType.Text)) > 0;
            }
        }
        /// <summary>
        /// Kiểm tra xem sản phẩm có đang được sử dụng trong bảng OrderDetails hay không
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> InUsedAsync(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT CASE WHEN EXISTS(SELECT 1 FROM OrderDetails WHERE ProductID = @id)
                                         THEN 1 ELSE 0 END;";
                return await connection.ExecuteScalarAsync<bool>(sql: sql, param: new { id }, commandType: CommandType.Text);
            }
        }
        /// <summary>
        /// Lấy danh sách Attribute của 1 sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<ProductAttribute>> ListAttributesAsync(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT * 
                    FROM ProductAttributes
                    WHERE ProductID = @ProductID
                    ORDER BY DisplayOrder";
                var parameters = new
                {
                    id
                };
                return await connection.QueryAsync<ProductAttribute>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
        }
        /// <summary>
        /// Lấy 1 attribute theo ID
        /// </summary>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ProductAttribute?> GetAttributeAsync(long attributeID)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT * FROM ProductAttribute WHERE AttributeID = @AttributeID";
                var parameters = new
                {
                    attributeID
                };
                return await connection.QueryFirstOrDefaultAsync<ProductAttribute?>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
        }
        /// <summary>
        /// Thêm Attribute mới
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<long> AddAttributeAsync(ProductAttribute data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"INSERT INTO ProductAttributes 
                            (
                                ProductID, 
                                AttributeName, 
                                AttributeValue, 
                                DisplayOrder
                            ) 
                            VALUES 
                            (
                                @ProductID, 
                                @AttributeName, 
                                @AttributeValue, 
                                @DisplayOrder
                            )
                            SELECT SCOPE_IDENTITY();";
                return await connection.ExecuteScalarAsync<long>(sql: sql, param: data, commandType: CommandType.Text);
            }
        }
        /// <summary>
        /// Cập nhật Attribute
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateAttributeAsync(ProductAttribute data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"UPDATE ProductAttributes 
                            SET AttributeName = @AttributeName,
                                AttributeValue = @AttributeValue,
                                DisplayOrder = @DisplayOrder
                            WHERE AttributeID = @AttributeID;";

                return await connection.ExecuteAsync(sql: sql, param: data, commandType: CommandType.Text) > 0;
            }
        }
        /// <summary>
        /// Xóa Attribute
        /// </summary>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteAttributeAsync(long attributeID)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"DELETE FROM ProductAttributes WHERE AttributeID = @attributeId";
                var parameters = new
                {
                    attributeID
                };
                return await connection.ExecuteAsync(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
            }
        }
        /// <summary>
        /// Lấy danh sách ảnh của sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<ProductPhoto>> ListPhotosAsync(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT * FROM ProductPhotos
                                WHERE ProductID = @productId
                            ORDER BY DisplayOrder";
                var parameters = new { id };
                return await connection.QueryAsync<ProductPhoto>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
        }
        /// <summary>
        /// Lấy 1 ảnh theo PhotoID
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ProductPhoto?> GetPhotoAsync(long photoID)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT * FROM ProductPhotos
                                WHERE PhotoID = @photoId";
                var parameters = new
                {
                    photoID
                };
                return await connection.QueryFirstOrDefaultAsync<ProductPhoto>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
        }
        /// <summary>
        /// Thêm ảnh mới cho sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<long> AddPhotoAsync(ProductPhoto data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"INSERT INTO ProductPhotos
                            (
                                ProductID,
                                Photo,
                                Description,
                                DisplayOrder,
                                IsHidden
                            )
                            VALUES
                            (
                                @ProductID,
                                @Photo,
                                @Description,
                                @DisplayOrder,
                                @IsHidden
                            );
                            SELECT SCOPE_IDENTITY();";
                return await connection.ExecuteScalarAsync<long>(sql: sql, param: data, commandType: CommandType.Text);
            }
        }
        /// <summary>
        /// Cập nhật ảnh
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdatePhotoAsync(ProductPhoto data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"UPDATE ProductPhotos
                            SET Photo = @Photo,
                                Description = @Description,
                                DisplayOrder = @DisplayOrder,
                                IsHidden = @IsHidden
                            WHERE PhotoID = @PhotoID;";
                return await connection.ExecuteAsync(sql: sql, param: data, commandType: CommandType.Text) > 0;
            }
        }
        /// <summary>
        /// Xóa 1 ảnh theo PhotoID
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeletePhotoAsync(long photoID)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"DELETE FROM ProductPhotos WHERE PhotoID = @photoId";
                var parameters = new
                {
                    photoID
                };
                return await connection.ExecuteAsync(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
            }
        }
    }
}

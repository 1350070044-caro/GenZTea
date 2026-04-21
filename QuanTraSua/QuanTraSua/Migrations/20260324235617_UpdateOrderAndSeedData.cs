using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanTraSua.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Đậm vị trà ô long, trân châu dai giòn.", "https://tocotocotea.com/wp-content/uploads/2021/12/O-Long-Thai-Cuc.png", "Trà Sữa Trân Châu Ô Long", 35000m },
                    { 2, "Bột matcha nguyên chất từ Nhật.", "https://tocotocotea.com/wp-content/uploads/2021/12/Tra-Sua-Matcha.png", "Trà Sữa Matcha Nhật Bản", 40000m },
                    { 3, "Sữa tươi nguyên chất mix đường đen.", "https://tocotocotea.com/wp-content/uploads/2021/12/Sua-Tuoi-Tran-Chau-Duong-Den.png", "Sữa Tươi Trân Châu Đường Đen", 45000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimDongDoi.API.Migrations
{
    /// <inheritdoc />
    public partial class FinalSchemaUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ⭐ HÀNH ĐỘNG 1: XÓA CỘT 'password' CŨ (Gây lỗi 'Cannot insert NULL')
            // Lệnh này loại bỏ cột password cũ khỏi bảng users.
            migrationBuilder.DropColumn(
                name: "password", 
                table: "users");

            // ⭐ HÀNH ĐỘNG 2: THÊM CỘT PasswordHash MỚI
            // Lệnh này thêm cột PasswordHash mới vào bảng users.
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "users",
                type: "varbinary(max)",
                nullable: false, 
                defaultValue: new byte[0]); 

            // ⭐ HÀNH ĐỘNG 3: THÊM CỘT PasswordSalt MỚI
            // Lệnh này thêm cột PasswordSalt mới vào bảng users.
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
            
            // LƯU Ý: TẤT CẢ LỆNH CreateTable, CreateIndex, v.v., ĐÃ BỊ XÓA TẠI ĐÂY.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // ⭐ HOÀN TÁC: Xóa các cột mới
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "users");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "users");

            // ⭐ HOÀN TÁC: Thêm lại cột password cũ (với giá trị mặc định tạm thời)
            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "users",
                type: "nvarchar(255)", 
                maxLength: 255, 
                nullable: false,
                defaultValue: "temp_password");
            
            // TẤT CẢ LỆNH DropTable, DropIndex KHÁC ĐÃ BỊ XÓA TẠI ĐÂY.
        }
    }
}

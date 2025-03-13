using System;
using Bogus;
using Microsoft.EntityFrameworkCore.Migrations;
using RazorPage.Models;

#nullable disable

namespace RazorPage.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articles", x => x.Id);
                });
            Randomizer.Seed = new Random(8675309);

            var fakerArticle = new Faker<Article>();
            fakerArticle.RuleFor(a => a.Title, f => f.Lorem.Sentence(5, 5));
            fakerArticle.RuleFor(a => a.Created, f => f.Date.Between(new DateTime(2024,01,01), new DateTime(2025,01,01)));
            fakerArticle.RuleFor(a => a.Content, f => f.Lorem.Paragraphs(1, 4));

           

            for(int i =0; i <150;i++)
            {
                Article articles = fakerArticle.Generate();
                migrationBuilder.InsertData(
                table: "articles",
                columns: new[] { "Title", "Created", "Content" },
                values: new object[]
                {
                    articles.Title,
                    articles.Created,
                    articles.Content
                }
            );
            }

            
            

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articles");
        }
    }
}

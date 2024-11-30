using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using hw_26._11._24.Data;
using hw_26._11._24.DTOs;
using hw_26._11._24.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace hw_26._11._24.Controllers
{
    public class ProductController : GraphController
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        [QueryRoot("products")]
        public async Task<IEnumerable<Product>> GetProducts(int offset = 0, int limit = 20, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Skip(offset)
                .Take(limit)
                .ToArrayAsync(cancellationToken);
        }

        [QueryRoot("getProduct")]
        public async Task<Product?> GetProduct(int id, CancellationToken cancellationToken)
        {
            return await _context.Products
                .FindAsync(keyValues: [id], cancellationToken: cancellationToken);
        }

        [MutationRoot("addProduct")]
        public async Task<Product> AddProduct(Product product)
        {
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        [MutationRoot("updateProduct")]
        public async Task<Product> UpdateProduct(int id, ProductUpdateDTO productUpdateDTO, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FindAsync(keyValues: [id], cancellationToken: cancellationToken);

            if (product is null)
            {
                throw new KeyNotFoundException($"There is no product with id: {id}");
            }

            product.Name = productUpdateDTO.Name ?? product.Name;
            product.Description = productUpdateDTO.Description ?? product.Description;
            product.Price = productUpdateDTO.Price ?? product.Price;

            await _context.SaveChangesAsync();

            return product;
        }

        [MutationRoot("deleteProduct")]
        public async Task<bool> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FindAsync(keyValues: [id], cancellationToken: cancellationToken);

            if (product is not null)
            {
                _context.Products.Remove(product);

                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}

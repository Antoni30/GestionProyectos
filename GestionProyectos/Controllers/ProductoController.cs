﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionProyectos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly AppDBContext _appDbContext;

        public ProductoController(AppDBContext appDBContext)
        {
            _appDbContext = appDBContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetProductos()
        {
            return Ok(await _appDbContext.Productos.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> CreateProducto(Producto producto)
        {
            if (producto.Precio < 0) return BadRequest("El prodcuto no puede ser negativo");
            _appDbContext.Productos.Add(producto);
            await _appDbContext.SaveChangesAsync();
            return Ok(producto);

        }
        [HttpPut]
        public async Task<IActionResult> EditarProducto(Producto producto)
        {
            if (producto.Precio < 0) return BadRequest("El precio del producto no puede ser negativo");
            var productoExistente = await _appDbContext.Productos.FindAsync(producto.Id);

            if (productoExistente == null) return NotFound("El producto no existe");

            productoExistente.Nombre = producto.Nombre;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Precio = producto.Precio;
            productoExistente.Stock = producto.Stock;

            await _appDbContext.SaveChangesAsync();

            return Ok(productoExistente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var productoExistente = await _appDbContext.Productos.FindAsync(id);

            if (productoExistente == null)
                return NotFound("El producto no existe");

            _appDbContext.Productos.Remove(productoExistente);

            await _appDbContext.SaveChangesAsync();

            return Ok($"Producto con ID {id} eliminado correctamente");
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Shop.Controllers
{
  [Route("products")]
  public class ProductController : ControllerBase
  {
    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context) //o task faz com que seja assíncrono e o action result retorna os status já padronizados
    {
      var products = await context
          .Products
          .Include(x => x.Category) // traz a categoria, se precisar só do Id pode tirar essa linha, ela faz um join na tabela de categoria pra trazer esses dados (da pra incluir outros includes tb se precisar)
          .AsNoTracking()
          .ToListAsync();

      if (products.Count == 0)
        return NoContent();
      return Ok(products);
    }

    [HttpGet]
    [Route("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Product>> GetById(
        int id,
        [FromServices] DataContext context)
    {
      var product = await context
          .Products
          .Include(x => x.Category)
          .AsNoTracking()
          .ToListAsync();

      if (product == null)
        return NotFound(new { message = "Categoria não encontrada" });

      return Ok(product);
    }

    [HttpGet]
    [Route("categories/{id:int}")] // lista todos os produtos que possuem uma determinada categoria
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Product>> GetByCategory(
        int id,
        [FromServices] DataContext context)
    {
      var product = await context
          .Products
          .Include(x => x.Category)
          .AsNoTracking()
          .Where(x => x.CategoryId == id)
          .ToListAsync();

      if (product == null)
        return NotFound(new { message = $"Não existem produtos com a categoria {id}" });

      return Ok(product);
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<List<Product>>> Post(
        [FromBody] Product model,
        [FromServices] DataContext context)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);
      try
      {
        context.Products.Add(model);
        await context.SaveChangesAsync(); // gera id automatico e ja preenche o id do model
        return Ok(model);

      }
      catch
      {
        return BadRequest(new { message = "Não foi possível criar a categoria" });
      }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<List<Product>>> Put(
        int id,
        [FromBody] Product model,
        [FromServices] DataContext context)
    {
      if (model.Id != id)
      {
        return NotFound(new { message = "Categoria não encontrada" });
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        context.Entry<Product>(model).State = EntityState.Modified; // o EF altera automaticamente o que foi alterado baseado no model de entrada
        await context.SaveChangesAsync(); // gera id automatico e ja preenche o id do model
        return Ok(model);

      }
      catch (DbUpdateConcurrencyException) // quando o mesmo registro é atualizado ao mesmo tempo
      {
        return BadRequest(new { message = "Registro já atualizado" });
      }
      catch
      {
        return BadRequest(new { message = "Não foi possível atualizar a categoria" });
      }

    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<List<Product>>> Delete(
        int id,
        [FromServices] DataContext context)
    {
      var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
      if (product == null)
        return NotFound(new { message = "Categoria não encontrada" });

      try
      {
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return Ok(product);
      }
      catch (Exception)
      {

        return BadRequest(new { message = "Categoria não removida" });
      }
    }
  }
}

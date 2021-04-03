using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Controllers
{
  // https://localhost:5001 => pra https o padrao é 5001 e pra http 5000
  // https://meuapp.azurewebsites.net/ => quando subir para o azure

  [Route("v1/categories")]
  public class CategoryController : ControllerBase
  {
    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context) //o task faz com que seja assíncrono e o action result retorna os status já padronizados
    {
      var categories = await context.Categories.AsNoTracking().ToListAsync();
      if (categories.Count == 0)
        return NoContent();
      return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Category>> GetById(
        int id,
        [FromServices] DataContext context)
    {
      var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
      if (category == null)
        return NotFound(new { message = "Categoria não encontrada" });

      return Ok(category);
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<List<Category>>> Post(
        [FromBody] Category model,
        [FromServices] DataContext context)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);
      try
      {
        context.Categories.Add(model);
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
    public async Task<ActionResult<List<Category>>> Put(
        int id,
        [FromBody] Category model,
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
        context.Entry<Category>(model).State = EntityState.Modified; // o EF altera automaticamente o que foi alterado baseado no model de entrada
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
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<List<Category>>> Delete(
        int id,
        [FromServices] DataContext context)
    {
      var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
      if (category == null)
        return NotFound(new { message = "Categoria não encontrada" });

      try
      {
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return Ok(category);
      }
      catch (Exception)
      {

        return BadRequest(new { message = "Categoria não removida" });
      }
    }

  }
}

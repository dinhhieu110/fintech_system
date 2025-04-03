using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.dtos.comment;
using server.interfaces;
using server.mappers;

namespace server.controllers
  {
  [Route("api/comments")]
  [ApiController]
  public class CommentController : ControllerBase
  {
    private readonly ICommentRepository _commentRepo;

    public CommentController(ICommentRepository commentRepo)
    {
      _commentRepo = commentRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var comments = await _commentRepo.GetAllAsync();

      var commentDTOs = comments.Select(i => i.ToCommentDTO());

      return Ok(commentDTOs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
      var comment = await _commentRepo.GetByIdAsync(id);
      if (comment == null)
      {
        return NotFound();
      }
      return Ok(comment);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCommentReqDTO newComment)
    {
      var commentModel = newComment.ToCommentFromCreateDTO();
      await _commentRepo.CreateAsync(commentModel);
      return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDTO());

    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatedReqDTO updatedComment)
    {
      var commentModel = await _commentRepo.UpdateAsync(id, updatedComment);
      if (commentModel == null)
      {
        return NotFound();
      }
      return Ok(commentModel.ToCommentDTO());

    }

     [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
      var commentModel = await _commentRepo.DeleteAsync(id);
      if (commentModel == null)
      {
        return NotFound();
      }
      return NoContent();

    }
  }
}

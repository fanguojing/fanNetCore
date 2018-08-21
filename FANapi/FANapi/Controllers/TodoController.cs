using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FANapi.Models;

namespace FANapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        public readonly TodoContext _todoContext;
        public TodoController(TodoContext todoContext)
        {
            _todoContext = todoContext;
            if (_todoContext.TodoItems.Count()==0)
            {
                _todoContext.TodoItems.Add(new TodoItem { Name = "item1" });
                _todoContext.SaveChanges(); 
            }
            
        }
        // GET: api/Todo
        [HttpGet]
        public IEnumerable<TodoItem> Get()
        {
            return _todoContext.TodoItems.ToList();
        }

        // GET: api/Todo/5  name属性
        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult Get(int id)
        {
            var todo = _todoContext.TodoItems.FirstOrDefault(u => u.Id == id);
            if(todo==null)
            {
                return NotFound();
            }
            return new ObjectResult(todo);
        }

        // POST: api/Todo
        [HttpPost]
        public IActionResult Post([FromBody] TodoItem todo)
        {
            if(todo==null)
            {
                return BadRequest();
            }
            _todoContext.TodoItems.Add(todo);
            _todoContext.SaveChanges();
            return CreatedAtRoute("GetTodo", new { id = todo.Id }, todo);
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TodoItem todo)
        {
            if(todo==null || todo.Id !=id)
            {
                return BadRequest();
            }
            var item = _todoContext.TodoItems.FirstOrDefault(u => u.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            item.IsCompelete = todo.IsCompelete;
            item.Name = todo.Name;
            _todoContext.TodoItems.Update(item);
            _todoContext.SaveChanges();
            return new NoContentResult();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todo = _todoContext.TodoItems.FirstOrDefault(u => u.Id == id);
            if(todo==null)
            {
                return NotFound();
            }
            _todoContext.TodoItems.Remove(todo);
            _todoContext.SaveChanges();
            return new NoContentResult();
        }
    }
}

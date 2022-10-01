using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using APITaskManagerCSharpDotNet.Models;
using APITaskManagerCSharpDotNet.Context;
using APITaskManagerCSharpDotNet.Enum;

namespace APITaskManagerCSharpDotNet.Controllers
{
    [ApiController]
    [Route("tarefa")]

    public class TaskitemController:ControllerBase
    {
        private TaskManagerContext _context { get; }

        public TaskitemController(TaskManagerContext context)
        {
            this._context = context;            
        }

        [HttpGet("obtertodos")]
        public ActionResult<List<TaskListItem>> Get(){

            var taskList = _context.TaskList.ToList();

            if (!taskList.Any()){

                return NoContent();
            }

            return Ok(taskList);
        }

        [HttpGet("obterporstatus/{status}")]
        public ActionResult<List<TaskListItem>> Get([FromRoute]int status){

            var taskList = _context.TaskList.Where( t => t.Status == (TaskItemStatus) status).ToList();

            if (!taskList.Any()){

                return NoContent();
            }

            return Ok(taskList);
        }

        [HttpGet("obterpordata/{data}")]
        public ActionResult<List<TaskListItem>> Get([FromRoute] DateTime data){

            var taskList = _context.TaskList.Where( t => t.Data == data).ToList();

            if (!taskList.Any()){

                return NoContent();
            }

            return Ok(taskList);
        }

        [HttpGet("obterportitulo/{titulo}")]
        public ActionResult<List<TaskListItem>> Get([FromRoute] string titulo){

            var taskList = _context.TaskList.Where( t => t.Title == titulo).ToList();

            if (!taskList.Any()){

                return NoContent();
            }

            return Ok(taskList);
        }        

        [HttpPost]
        public ActionResult<TaskListItem> Post([FromBody] TaskListItem newTask){
            try{

                _context.TaskList.Add(newTask);
                _context.SaveChanges();

                return Created("Tarefa adicionada.", newTask);

            } catch (Exception e) {

                return BadRequest( new{ msg = "Não foi possível adicionar tarefa." + e.Message,
                                        status = HttpStatusCode.BadRequest }
                            );
            }           

        }

        [HttpPut("{id}")]
        public ActionResult Put([FromRoute] int id, [FromBody] TaskListItem alteredTask)
        {
            var taskForUpdate = _context.TaskList.FirstOrDefault(t => t.Id == id);

            if (taskForUpdate is null){
                
                return NoContent();
            }

            try {

                taskForUpdate.Status      = alteredTask.Status;
                taskForUpdate.Description = alteredTask.Description;
                taskForUpdate.Title       = alteredTask.Title;

                _context.TaskList.Update(taskForUpdate);
                _context.SaveChanges();

                return Ok(new { msg    = $"Tarefa atualzida com sucesso, {id}",
                                status = HttpStatusCode.OK,
                                taskForUpdate });

             } catch (Exception e){

                return BadRequest( new { msg = $"Erro ao atualizar a tarefa, {id}" + e.Message,
                                         status = HttpStatusCode.BadRequest});

             }            
        }

        [HttpDelete("{id}")]

        public ActionResult Delete([FromRoute] int id)
        {
            try{
                var taskForDelete = _context.TaskList.FirstOrDefault( t => t.Id == id);

                if (taskForDelete is null){
                    return BadRequest( new { msg = $"Tarefa não localizada para exclusão, {id}",
                                             status = HttpStatusCode.BadRequest });
                };

                _context.TaskList.Remove(taskForDelete);
                _context.SaveChanges();

                return Ok( new { msg = $"Tarefa removida, {id}.",
                                 status = HttpStatusCode.OK,
                                 taskForDelete });

            }catch (Exception e) {
                return BadRequest( new { msg   = $"Não foi possível deletar a tarefa, {id}."+ e.Message,
                                        status = HttpStatusCode.BadRequest });

            }
        }
    }
}
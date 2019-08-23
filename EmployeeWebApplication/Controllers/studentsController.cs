using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EmployeeWebApplication.Models;

namespace EmployeeWebApplication.Controllers
{
    public class studentsController : ApiController
    {
        private studentdbEntities db = new studentdbEntities();

        // GET: api/students
        public IQueryable<student> Getstudents()
        {
            return db.students;
        }

        // GET: api/students/5
        [ResponseType(typeof(student))]
        public async Task<IHttpActionResult> Getstudent(int id)
        {
            student student = await db.students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // PUT: api/students/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putstudent(int id, student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.S_ID)
            {
                return BadRequest();
            }

            db.Entry(student).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!studentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/students
        [ResponseType(typeof(student))]
        public async Task<IHttpActionResult> Poststudent(student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.students.Add(student);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (studentExists(student.S_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = student.S_ID }, student);
        }

        // DELETE: api/students/5
        [ResponseType(typeof(student))]
        public async Task<IHttpActionResult> Deletestudent(int id)
        {
            student student = await db.students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            db.students.Remove(student);
            await db.SaveChangesAsync();

            return Ok(student);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool studentExists(int id)
        {
            return db.students.Count(e => e.S_ID == id) > 0;
        }
    }
}
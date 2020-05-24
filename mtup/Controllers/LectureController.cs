using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using mtup.Entities;
using mtup.Models;

namespace mtup.Controllers
{
    [Route("szym/meetup/{meetupName}/lecture")]
    public class LectureController : ControllerBase
    {
        private MeetupContext _meetupContext;
        private readonly IMapper _mapper;
        private readonly ILogger<LectureController> _logger;

        public LectureController(MeetupContext meetupContext, IMapper mapper, ILogger<LectureController> logger)
        {
            _meetupContext = meetupContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult Post(string meetupName, [FromBody] LectureDto lectureModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var meetup = _meetupContext.Meetups
                .Include(m => m.Lectures)
                .FirstOrDefault(m => m.Name.Replace(" ","-").ToLower() == meetupName.ToLower());
            
            if (meetup == null) return NotFound(meetupName);

            var lecture = _mapper.Map<Lecture>(lectureModel);
            meetup.Lectures.Add(lecture);
            _meetupContext.SaveChanges();

            return Created($"api/{meetupName}/lecture/{lecture.Topic}", null);
        }

        [HttpGet]
        public ActionResult Get (string meetupName)
        {
            var meetup = _meetupContext.Meetups
                .Include(m => m.Lectures)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

            if (meetup == null) return NotFound();

            var lectures = _mapper.Map<List<LectureDto>>(meetup.Lectures);

            return Ok(lectures);
        }

        [HttpDelete]
        public ActionResult Delete(string meetupName)
        {
            var meetup = _meetupContext.Meetups
                .Include(m => m.Lectures)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

            if (meetup == null) return NotFound();

            _meetupContext.Lectures.RemoveRange(meetup.Lectures);
            _meetupContext.SaveChanges();

            _logger.LogWarning($"no i sru, wszystkie z {meetup.Name} poszły ...");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string meetupName, int id)
        {
            var meetup = _meetupContext.Meetups
                    .Include(m => m.Lectures)
                    .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower()==meetupName.ToLower());
            if (meetup == null) return NotFound();

            var lecture = meetup.Lectures.FirstOrDefault(l => l.Id == id);
            if (lecture == null) return NotFound();

            _meetupContext.Remove(lecture);
            _meetupContext.SaveChanges();

            return NoContent();

        }
    }
}
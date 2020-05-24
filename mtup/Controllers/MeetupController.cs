using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mtup.Entities;
using mtup.Models;

namespace mtup.Controllers
{
    [Route("szym/[controller]")]
    public class MeetupController : ControllerBase
    {
        private MeetupContext _meetupContext;
        private readonly IMapper _mapper;

        public MeetupController(MeetupContext meetupContext, IMapper mapper)
        {
            _meetupContext = meetupContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<MeetupDetailsDto>> Get()
        {

            var meetups = _meetupContext.Meetups.Include(m => m.Location).ToList();
            var meetupDtos = _mapper.Map<List<MeetupDetailsDto>>(meetups);

            return Ok(meetupDtos);

            //return NotFound(_meetupContext.Meetups.ToList());

            //return StatusCode(301, _meetupContext.Meetups.ToList());

            //return Ok(_meetupContext.Meetups.ToList());

            //return _meetupContext.Meetups.ToList();
        }

        [HttpGet("{name}")]
        public ActionResult<MeetupDetailsDto> Get(string name)
        {
            var meetup = _meetupContext.Meetups
                .Include(m => m.Location)
                .Include(m => m.Lectures)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());
            if (meetup == null) return NotFound("i dupa");

            var meetupDto = _mapper.Map<MeetupDetailsDto>(meetup);

            return Ok(meetupDto);
        }

        [HttpPost]
        public ActionResult Post([FromBody] MeetupDto newMeetup)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState.Values);

            var meetup = _mapper.Map<Meetup>(newMeetup);
            _meetupContext.Meetups.Add(meetup);
            _meetupContext.SaveChanges();

            return Created($"szym/meetup/{meetup.Name.Replace(" ", "-").ToLower()}", null);
        }

        [HttpPut("{name}")]
        public ActionResult Put(string name, [FromBody] MeetupDto updatedMeetup)
        {
            var meetup = _meetupContext.Meetups.FirstOrDefault(m => m.Name.Replace(" ", "-" ).ToLower() == name.ToLower());

            if (meetup == null) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            meetup.Name = updatedMeetup.Name.Replace("-", " ");
            meetup.Organizer = updatedMeetup.Organizer;
            meetup.Date = updatedMeetup.Date;
            meetup.IsPrivate = true;

            _meetupContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{name}")]
        public ActionResult Delete(string name)
        {
            var meetup = _meetupContext.Meetups.FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

            if (meetup == null) return NotFound();

            _meetupContext.Meetups.Remove(meetup);
            _meetupContext.SaveChanges();

            return NoContent();
        }

    }
}
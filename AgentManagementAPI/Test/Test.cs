

// GET: api/Missions/5
[HttpGet("{id}")]
public async Task<ActionResult<Mission>> GetMission(int id)
{
    var mission = await _context.Missions.FindAsync(id);

    if (mission == null)
    {
        return NotFound();
    }

    return mission;
}

// PUT: api/Missions/5
[HttpPut("{id}")]
public async Task<IActionResult> PutMission(int id, Mission mission)
{
    if (id != mission.Id)
    {
        return BadRequest();
    }

    _context.Entry(mission).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!MissionExists(id))
        {
            return NotFound();
        }
        else
        {
            throw;
        }
    }

    return NoContent();
}

// POST: api/Missions
[HttpPost]
public async Task<ActionResult<Mission>> PostMission(Mission mission)
{
    _context.Missions.Add(mission);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetMission", new { id = mission.Id }, mission);
}

// DELETE: api/Missions/5
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteMission(int id)
{
    var mission = await _context.Missions.FindAsync(id);
    if (mission == null)
    {
        return NotFound();
    }

    _context.Missions.Remove(mission);
    await _context.SaveChangesAsync();

    return NoContent();
}

private bool MissionExists(int id)
{
    return _context.Missions.Any(e => e.Id == id);
}
    }









public async Task<IActionResult> DeleteAgent(int id)
            return NoContent();
        }

        //PUT: api/5/pin
        [HttpPut("{id}/pin")]
public IActionResult SetInitialPosition(int id, [FromBody] PinLocation location)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }
    var agent = _context.Targets.Find(id);
    if (agent == null)
    {
        return NotFound();
    }

    if (agent.Location == null)
    {
        agent.Location = new PinLocation();
    }
    agent.Location.X = location.X;
    agent.Location.Y = location.Y;

    _context.SaveChanges();

    return Ok(agent);
}

//PUT: api/5/move
[HttpPut("{id}/move")]
public IActionResult MoveAgent(int id, [FromBody] string directionString)
{
    var agent = _context.Targets.Find(id);
    if (agent == null)
    {
        return NotFound();
    }

    if (!Enum.TryParse<Direction>(directionString, true, out var direction))
    {
        return BadRequest("Invalid direction value.");
    }

    if (agent.Location == null)
    {
        agent.Location = new PinLocation();
    }

    switch (direction)
    {
        case Direction.NW:
            agent.Location.X -= 1;
            agent.Location.Y += 1;
            break;
        case Direction.N:
            agent.Location.Y += 1;
            break;
        case Direction.NE:
            agent.Location.X += 1;
            agent.Location.Y += 1;
            break;
        case Direction.W:
            agent.Location.X -= 1;
            break;
        case Direction.E:
            agent.Location.X += 1;
            break;
        case Direction.SW:
            agent.Location.X -= 1;
            agent.Location.Y -= 1;
            break;
        case Direction.S:
            agent.Location.Y -= 1;
            break;
        case Direction.SE:
            agent.Location.X += 1;
            agent.Location.Y -= 1;
            break;
    }

    _context.SaveChanges();
    return Ok(agent);
}


private bool AgentExists(int id)
{
    return _context.Agents.Any(e => e.Id == id);
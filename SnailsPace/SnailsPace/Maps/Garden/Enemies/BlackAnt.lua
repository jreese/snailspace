
--[[ 
	BlackAnt.lua
	Define the Black Ant's properties and behaviors.
]]--

library('AI')

-- Generic BlackAntImage object to be reused by all Black Ants
BlackAntImage = Image()
BlackAntImage.filename = "Resources/Textures/BlackAntTable"
BlackAntImage.blocks = Vector2(4, 4)
BlackAntImage.size = Vector2(128, 128)

-- Creates a Sprite for a BlackAnt
function BASprite(animSt, animEnd, animDelay)
	sprt = Sprite()
	sprt.image = BlackAntImage
	sprt.effect = "Resources/Effects/effects"
	sprt.visible = true
	sprt.animationStart = animSt
	sprt.animationEnd = animEnd
	sprt.animationDelay = animDelay
	sprt.frame = 0
	sprt.timer = 0
	
	return sprt
end

-- Creates a BlackAnt object
function BlackAnt(startPos, behav)
	walk = BASprite(0, 3, 0.07)
	stand = BASprite(0, 0, 0.07)
	die = BASprite(8, 11, .1)
	
	blackant = Character()
	blackant.sprites:Add("Walk", walk)
	blackant.sprites:Add("Stand", stand)
	blackant.sprites:Add("Die", die)
	blackant.size = Vector2(BlackAntImage.size.X, BlackAntImage.size.Y - 64)
	blackant.startPosition = startPos
	blackant.position = startPos
	blackant.affectedByGravity = true
	blackant.maxVelocity = 640
	blackant.thinker = "BlackAntThinker"
	blackant.health = 1
	blackant.name = "Black Ant"
	blackant:setSprite("Stand")
	blackant.state = {
		tracking = false,
		mad = false,
	}
	blackant.behavior = behav
	map.characters:Add(blackant)
	
	return blackant
end

-- Black Ant behavior function
function BlackAntThinker( self, gameTime )
	if (self.behavior == "platPatrol") then
		AI.platformPatrol(self)
		if(AI.canSeeHelix(self, 300)) then
			AI.shootDirectlyAtHelix(self, gameTime)
		end
	elseif (self.behavior == "patrol") then
		AI.patrol(self, self.startPosition.X + 300, self.startPosition.X - 300)
		if(AI.canSeeHelix(self, 300)) then
			AI.shootDirectlyAtHelix(self, gameTime)
		end
	elseif (self.behavior == "attack") then
		self:setSprite("Walk")
		self.maxVelocity = 200
		AI.moveToHelix(self, nil, nil, nil, false)
	elseif(self.behavior == "attackAndShoot") then
		self:setSprite("Walk")
		if(AI.canSeeHelix(self, 300)) then
			AI.shootDirectlyAtHelix(self, gameTime)
		end
	end
end
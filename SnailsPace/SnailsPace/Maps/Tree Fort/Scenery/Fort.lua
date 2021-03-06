xOffset = 0
yOffset = 3136

wallHeight = 4

-- Background
rotmod = 0.25
for x=0,25 do
	for y=0,wallHeight * 3.5 do
		if x < 16 or x > 21 or y > wallHeight * 2.4 or y < wallHeight * 1 then
			WorldBuilding.BuildObject( { xOffset=2.0 * -woodSprite.image.size.X + plywoodImage.size.X * x * 0.5, yOffset=128 + plywoodImage.size.Y * y * 0.5, sprite=plywoodSprite, collidable=false, rotation=newRotation() } )
		end
	end
end

rotmod = 5
-- Floor
WorldBuilding.BuildObject( { xOffset=2.2 * -woodSprite.image.size.X, yOffset=0, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=newRotation() } )
WorldBuilding.BuildObject( { xOffset=1.8 * -woodSprite.image.size.X, yOffset=0, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=newRotation() } )
WorldBuilding.BuildObject( { xOffset=-woodSprite.image.size.X, yOffset=0, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=newRotation() } )
WorldBuilding.BuildObject( { xOffset=woodSprite.image.size.X, yOffset=0, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=newRotation() } )
WorldBuilding.BuildObject( { xOffset=1.8 * woodSprite.image.size.X, yOffset=0, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=newRotation() } )
WorldBuilding.BuildObject( { xOffset=2.6 * woodSprite.image.size.X, yOffset=0, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=newRotation() } )
WorldBuilding.BuildObject( { xOffset=3.4 * woodSprite.image.size.X, yOffset=0, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=newRotation() } )
WorldBuilding.BuildObject( { xOffset=4.2 * woodSprite.image.size.X, yOffset=0, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=newRotation() } )
WorldBuilding.BuildObject( { xOffset=4.6 * woodSprite.image.size.X, yOffset=0, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=newRotation() } )

-- Side walls
for i=0,wallHeight - 1 do
	-- Left
	WorldBuilding.BuildObject( { xOffset=-2.3 * woodSprite.image.size.X, yOffset=( 0.4 + 0.8 * i ) * woodSprite.image.size.X, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=MathHelper.PiOver2 + newRotation() } )

	-- Right
	WorldBuilding.BuildObject( { xOffset=4.7 * woodSprite.image.size.X, yOffset=( 0.4 + 0.8 * i ) * woodSprite.image.size.X, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=MathHelper.PiOver2 + newRotation() } )
end
-- Right
WorldBuilding.BuildObject( { xOffset=4.7 * woodSprite.image.size.X, yOffset=( 0.4 + 0.8 * wallHeight ) * woodSprite.image.size.X, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=MathHelper.PiOver2 + newRotation() } )


-- Platforms leading to the Hive

plat = GameObject()
plat.sprites:Add("wood", woodSprite)
plat.size = woodImage.size
plat.collidable = true
plat.position = Vector2(xOffset - 150, yOffset + 300)
map.objects:Add(plat)

SavePoints.BuildSavePoint( xOffset + -150, yOffset - 2730 )

plat = GameObject()
plat.sprites:Add("wood", woodSprite)
plat.size = woodImage.size
plat.collidable = true
plat.position = Vector2(xOffset - 550, yOffset + 550)
map.objects:Add(plat)


plat = GameObject()
plat.sprites:Add("wood", woodSprite)
plat.size = woodImage.size
plat.collidable = true
plat.position = Vector2(xOffset + 450, yOffset + 1100)
map.objects:Add(plat)


-- Ceiling
for i=0,8 do
	WorldBuilding.BuildObject( { xOffset= ( -2.0 + 0.8 * i ) * woodSprite.image.size.X, yOffset=( 0.8 + 0.8 * wallHeight ) * woodSprite.image.size.X, sprite=woodSprite, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=newRotation() } )
end

-- Window
WorldBuilding.BuildObject( { xOffset=2.8 * woodSprite.image.size.X, yOffset=1.3 * woodSprite.image.size.X, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=newRotation() } )
WorldBuilding.BuildObject( { xOffset=2.6 * woodSprite.image.size.X, yOffset=2.6 * woodSprite.image.size.X, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=newRotation() } )
WorldBuilding.BuildObject( { xOffset=2.1 * woodSprite.image.size.X, yOffset=2.2 * woodSprite.image.size.X, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=MathHelper.PiOver2 + newRotation() } )
WorldBuilding.BuildObject( { xOffset=3.3 * woodSprite.image.size.X, yOffset=1.8 * woodSprite.image.size.X, sprite=woodSprite, xSizeMod=-16, ySizeMod=-16, rotation=MathHelper.PiOver2 + newRotation() } )
Spider(Vector2(xOffset + 1290, yOffset + 564))
Spider(Vector2(xOffset + 1400, yOffset + 1100))

Powerups.BuildWeaponPowerup( xOffset + 2150, 150, "flamethrower", 50 )

-- Boss
Hive( Vector2( xOffset - 1.8 * woodSprite.image.size.X, yOffset + ( 0.2 + 0.8 * wallHeight ) * woodSprite.image.size.X ) )
EndLevel.BuildLevelEnd( - 2.2 * woodSprite.image.size.X, ( 0.4 + 0.8 * wallHeight ) * woodSprite.image.size.X )

ssprite = savepointSprite:clone()
ssprite.frame = 2
ssprite.animationStart = 2
ssprite.animationEnd = 2

asprite = savepointSprite:clone()
asprite.frame = 3
asprite.animationStart = 3
asprite.animationEnd = 3

sign = GameObject()
sign.sprites:Add("sign", ssprite)
sign.sprites:Add("arrow", asprite)
sign.size = savepointImage.size
sign.collidable = false
sign.position = Vector2( -900, yOffset + 1900 )
sign.rotation = MathHelper.Pi
map.objects:Add(sign)

xOffset = 1536
yOffset = 0

WorldBuilding.BuildSection( { width=10, xOffset=0, yOffset=-192, sprite=grassSprite, xOverlap=20, yOverlap=20, xSizeMod=-32, ySizeMod=-32 } )
WorldBuilding.BuildSection( { width=20, xOffset=-48, yOffset=-320, sprite=dirtSpriteS, xOverlap=10, yOverlap=10, xSizeMod=-32, ySizeMod=-32, layerOffset=-2 } )
WorldBuilding.BuildSection( { width=19, xOffset=16, yOffset=-438, sprite=dirtSpriteS, xOverlap=10, yOverlap=10, xSizeMod=-32, ySizeMod=-32, layerOffset=-2, rotation=MathHelper.Pi } )
WorldBuilding.BuildSection( { width=18, xOffset=80, yOffset=-556, sprite=dirtSpriteS, xOverlap=10, yOverlap=10, xSizeMod=-32, ySizeMod=-32, layerOffset=-2 } )
WorldBuilding.BuildSection( { width=12, xOffset=448, yOffset=-674, sprite=dirtSpriteS, xOverlap=10, yOverlap=10, xSizeMod=-32, ySizeMod=-32, layerOffset=-2 } )
WorldBuilding.BuildRamp( {length=3, xOffset=-24, yOffset=-400, sprite=dirtSpriteS, overlap=10, rotation=-1.05, layerOffset=2 } )
WorldBuilding.BuildRamp( {length=6, xOffset=144, yOffset=-640, sprite=dirtSpriteS, overlap=10, rotation=-0.15, layerOffset=2 } )
WorldBuilding.BuildRamp( {length=12, xOffset=840, yOffset=-728, sprite=dirtSpriteS, overlap=10, rotation=0.05, layerOffset=2 } )
WorldBuilding.BuildObject( { xOffset=512, yOffset=fencePostImage.size.Y / 2 - 384, sprite=fencePostSprite, xSizeMod=-16, ySizeMod=0, rotation=0 } )
WorldBuilding.BuildObject( { xOffset=512, yOffset=fencePostImage.size.Y - 512 - 128, sprite=woodSprite, xSizeMod=-16, ySizeMod=0, rotation=0.4 } )
Powerups.BuildWeaponPowerup( 200,fencePostImage.size.Y - 512, "stinger", 100, 0.2 )

for i=0,8 do
	bee = Bee( Vector2( xOffset + 128, yOffset + 222 + i * 276 ), "swarmHelixA" )
	bee.state.attacking = true
	bee = Bee( Vector2( xOffset + 128, yOffset + 360 + i * 276 ), "swarmHelixB" )
	bee.state.attacking = true
end

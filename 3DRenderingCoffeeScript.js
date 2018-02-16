// CoffeeScript source code
// needs the following html tags
// <canvas width="1000" height="700" id="seen-canvas"></canvas>
// <script type="text/coffeescript" id="code"> </script>

width = 1000 // width of a scene
height = 700 // height of a scene

getData = -> data // this method should return JSON PlacingPlanRenderingModel object 

getData()

model = seen.Models.default()

model.add(seen.Shapes.rectangle(seen.P(0, 0, 0), seen.P(data.Container.Width, data.Container.Length, 2)))

for item in data.Boxes
    model.add(seen.Shapes.rectangle(seen.P(item.Xo + 3, item.Yo + 3, item.Zo + 3), seen.P(item.X1 - 3, item.Y1 - 3, item.Z1 - 3))
        .fill(item.Color))

scene = new seen.Scene
model: model.translate(-150, -50, 0).scale(1)
viewport: seen.Viewports.center(width, height)

context = seen.Context('seen-canvas', scene).render()

dragger = new seen.Drag('seen-canvas')
dragger.on('drag.rotate', (e) ->
    xform = seen.Quaternion.xyToTransform(e.offsetRelative...)
            model.transform(xform)
            context.render()
)
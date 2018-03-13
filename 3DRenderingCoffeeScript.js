// CoffeeScript source code
// needs the following html tags

//<table callpadding="0" cellspacing="0" border="0">
//<tr>
//<td rowspan="3"><canvas width="900" height="700" id="seen-canvas-0"></canvas></td>
//<td><canvas width="350" height="350" id="seen-canvas-1"></canvas></td>
//<td><canvas width="350" height="350" id="seen-canvas-2"></canvas></td>
//</tr>
//<tr>
//<td>Top view</td>
//<td>Front View</td>
//</tr>
//<tr>
//<td><canvas width="350" height="350" id="seen-canvas-3"></canvas></td>
//<td><canvas width="350" height="350" id="seen-canvas-4"></canvas></td>
//</tr>
//<tr>
//<td>Perspective view (drag to rotate model)</td>
//<td>Back view</td>
//<td>Right view</td>
//</tr>
//</table>
//<br /> <br />
//Input line to display: <input type="text" id="lineNumberInput" /> / <span id="linesTotalQuantity"></span> lines < br />
//<input type="button" id="filterRender" value="Show" />
//<input type="button" id="resetRender" value="Reset" /> <br />
//<span class="renderError"></span>

        $ ->
              dims = [
                  [900, 700]
                  [100, 600]
                  [600, 600]
                  [100, 550]
                  [600, 550]
              ]

              data = {}

              getData = ->
                  # TODO: rewrite with ajax request for PlacingPlanManager
                  data = { "Boxes": [{ "LineNumber": 1, "Xo": 6, "Yo": 6, "Zo": 6, "X1": 66, "Y1": 66, "Z1": 206, "Color": "#00897b", "Name": "A" }, { "LineNumber": 1, "Xo": 66, "Yo": 6, "Zo": 6, "X1": 126, "Y1": 66, "Z1": 206, "Color": "#00897b", "Name": "A" }, { "LineNumber": 1, "Xo": 126, "Yo": 6, "Zo": 6, "X1": 186, "Y1": 66, "Z1": 206, "Color": "#00897b", "Name": "A" }, { "LineNumber": 1, "Xo": 186, "Yo": 6, "Zo": 6, "X1": 246, "Y1": 66, "Z1": 206, "Color": "#00897b", "Name": "A" }, { "LineNumber": 1, "Xo": 246, "Yo": 6, "Zo": 6, "X1": 266, "Y1": 26, "Z1": 36, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 6, "Zo": 36, "X1": 266, "Y1": 26, "Z1": 66, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 6, "Zo": 66, "X1": 266, "Y1": 26, "Z1": 96, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 6, "Zo": 96, "X1": 266, "Y1": 26, "Z1": 126, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 6, "Zo": 126, "X1": 266, "Y1": 26, "Z1": 156, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 6, "Zo": 156, "X1": 266, "Y1": 26, "Z1": 186, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 26, "Zo": 6, "X1": 266, "Y1": 46, "Z1": 36, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 26, "Zo": 36, "X1": 266, "Y1": 46, "Z1": 66, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 26, "Zo": 66, "X1": 266, "Y1": 46, "Z1": 96, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 26, "Zo": 96, "X1": 266, "Y1": 46, "Z1": 126, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 26, "Zo": 126, "X1": 266, "Y1": 46, "Z1": 156, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 26, "Zo": 156, "X1": 266, "Y1": 46, "Z1": 186, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 46, "Zo": 6, "X1": 266, "Y1": 66, "Z1": 36, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 46, "Zo": 36, "X1": 266, "Y1": 66, "Z1": 66, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 46, "Zo": 66, "X1": 266, "Y1": 66, "Z1": 96, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 46, "Zo": 96, "X1": 266, "Y1": 66, "Z1": 126, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 46, "Zo": 126, "X1": 266, "Y1": 66, "Z1": 156, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 246, "Yo": 46, "Zo": 156, "X1": 266, "Y1": 66, "Z1": 186, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 6, "Zo": 6, "X1": 286, "Y1": 26, "Z1": 36, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 6, "Zo": 36, "X1": 286, "Y1": 26, "Z1": 66, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 6, "Zo": 66, "X1": 286, "Y1": 26, "Z1": 96, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 6, "Zo": 96, "X1": 286, "Y1": 26, "Z1": 126, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 6, "Zo": 126, "X1": 286, "Y1": 26, "Z1": 156, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 6, "Zo": 156, "X1": 286, "Y1": 26, "Z1": 186, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 26, "Zo": 6, "X1": 286, "Y1": 46, "Z1": 36, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 26, "Zo": 36, "X1": 286, "Y1": 46, "Z1": 66, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 26, "Zo": 66, "X1": 286, "Y1": 46, "Z1": 96, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 26, "Zo": 96, "X1": 286, "Y1": 46, "Z1": 126, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 26, "Zo": 126, "X1": 286, "Y1": 46, "Z1": 156, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 26, "Zo": 156, "X1": 286, "Y1": 46, "Z1": 186, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 46, "Zo": 6, "X1": 286, "Y1": 66, "Z1": 36, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 46, "Zo": 36, "X1": 286, "Y1": 66, "Z1": 66, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 46, "Zo": 66, "X1": 286, "Y1": 66, "Z1": 96, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 46, "Zo": 96, "X1": 286, "Y1": 66, "Z1": 126, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 46, "Zo": 126, "X1": 286, "Y1": 66, "Z1": 156, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 1, "Xo": 266, "Yo": 46, "Zo": 156, "X1": 286, "Y1": 66, "Z1": 186, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 2, "Xo": 6, "Yo": 66, "Zo": 6, "X1": 66, "Y1": 126, "Z1": 206, "Color": "#00897b", "Name": "A" }, { "LineNumber": 2, "Xo": 66, "Yo": 66, "Zo": 6, "X1": 126, "Y1": 126, "Z1": 206, "Color": "#00897b", "Name": "A" }, { "LineNumber": 2, "Xo": 126, "Yo": 66, "Zo": 6, "X1": 186, "Y1": 126, "Z1": 206, "Color": "#00897b", "Name": "A" }, { "LineNumber": 2, "Xo": 186, "Yo": 66, "Zo": 6, "X1": 286, "Y1": 121, "Z1": 86, "Color": "#00acc1", "Name": "B" }, { "LineNumber": 2, "Xo": 186, "Yo": 66, "Zo": 86, "X1": 286, "Y1": 121, "Z1": 166, "Color": "#00acc1", "Name": "B" }, { "LineNumber": 2, "Xo": 186, "Yo": 66, "Zo": 166, "X1": 206, "Y1": 86, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 2, "Xo": 186, "Yo": 86, "Zo": 166, "X1": 206, "Y1": 106, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 2, "Xo": 206, "Yo": 66, "Zo": 166, "X1": 226, "Y1": 86, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 2, "Xo": 206, "Yo": 86, "Zo": 166, "X1": 226, "Y1": 106, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 2, "Xo": 226, "Yo": 66, "Zo": 166, "X1": 246, "Y1": 86, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 2, "Xo": 226, "Yo": 86, "Zo": 166, "X1": 246, "Y1": 106, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 2, "Xo": 246, "Yo": 66, "Zo": 166, "X1": 266, "Y1": 86, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 2, "Xo": 246, "Yo": 86, "Zo": 166, "X1": 266, "Y1": 106, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 2, "Xo": 266, "Yo": 66, "Zo": 166, "X1": 286, "Y1": 86, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 2, "Xo": 266, "Yo": 86, "Zo": 166, "X1": 286, "Y1": 106, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 6, "Yo": 126, "Zo": 6, "X1": 61, "Y1": 226, "Z1": 86, "Color": "#00acc1", "Name": "B" }, { "LineNumber": 3, "Xo": 6, "Yo": 126, "Zo": 86, "X1": 61, "Y1": 226, "Z1": 166, "Color": "#00acc1", "Name": "B" }, { "LineNumber": 3, "Xo": 61, "Yo": 126, "Zo": 6, "X1": 116, "Y1": 226, "Z1": 86, "Color": "#00acc1", "Name": "B" }, { "LineNumber": 3, "Xo": 61, "Yo": 126, "Zo": 86, "X1": 116, "Y1": 226, "Z1": 166, "Color": "#00acc1", "Name": "B" }, { "LineNumber": 3, "Xo": 116, "Yo": 126, "Zo": 6, "X1": 171, "Y1": 226, "Z1": 86, "Color": "#00acc1", "Name": "B" }, { "LineNumber": 3, "Xo": 116, "Yo": 126, "Zo": 86, "X1": 171, "Y1": 226, "Z1": 166, "Color": "#00acc1", "Name": "B" }, { "LineNumber": 3, "Xo": 171, "Yo": 126, "Zo": 6, "X1": 226, "Y1": 226, "Z1": 86, "Color": "#00acc1", "Name": "B" }, { "LineNumber": 3, "Xo": 171, "Yo": 126, "Zo": 86, "X1": 226, "Y1": 226, "Z1": 166, "Color": "#00acc1", "Name": "B" }, { "LineNumber": 3, "Xo": 226, "Yo": 126, "Zo": 6, "X1": 281, "Y1": 226, "Z1": 86, "Color": "#00acc1", "Name": "B" }, { "LineNumber": 3, "Xo": 226, "Yo": 126, "Zo": 86, "X1": 281, "Y1": 226, "Z1": 166, "Color": "#00acc1", "Name": "B" }, { "LineNumber": 3, "Xo": 6, "Yo": 126, "Zo": 166, "X1": 26, "Y1": 146, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 6, "Yo": 146, "Zo": 166, "X1": 26, "Y1": 166, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 6, "Yo": 166, "Zo": 166, "X1": 26, "Y1": 186, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 6, "Yo": 186, "Zo": 166, "X1": 26, "Y1": 206, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 6, "Yo": 206, "Zo": 166, "X1": 26, "Y1": 226, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 26, "Yo": 126, "Zo": 166, "X1": 46, "Y1": 146, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 26, "Yo": 146, "Zo": 166, "X1": 46, "Y1": 166, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 26, "Yo": 166, "Zo": 166, "X1": 46, "Y1": 186, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 26, "Yo": 186, "Zo": 166, "X1": 46, "Y1": 206, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 26, "Yo": 206, "Zo": 166, "X1": 46, "Y1": 226, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 46, "Yo": 126, "Zo": 166, "X1": 66, "Y1": 146, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 46, "Yo": 146, "Zo": 166, "X1": 66, "Y1": 166, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 46, "Yo": 166, "Zo": 166, "X1": 66, "Y1": 186, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 46, "Yo": 186, "Zo": 166, "X1": 66, "Y1": 206, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 46, "Yo": 206, "Zo": 166, "X1": 66, "Y1": 226, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 66, "Yo": 126, "Zo": 166, "X1": 86, "Y1": 146, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 66, "Yo": 146, "Zo": 166, "X1": 86, "Y1": 166, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 66, "Yo": 166, "Zo": 166, "X1": 86, "Y1": 186, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 66, "Yo": 186, "Zo": 166, "X1": 86, "Y1": 206, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 66, "Yo": 206, "Zo": 166, "X1": 86, "Y1": 226, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 86, "Yo": 126, "Zo": 166, "X1": 106, "Y1": 146, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 86, "Yo": 146, "Zo": 166, "X1": 106, "Y1": 166, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 86, "Yo": 166, "Zo": 166, "X1": 106, "Y1": 186, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 86, "Yo": 186, "Zo": 166, "X1": 106, "Y1": 206, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 86, "Yo": 206, "Zo": 166, "X1": 106, "Y1": 226, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 106, "Yo": 126, "Zo": 166, "X1": 126, "Y1": 146, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 106, "Yo": 146, "Zo": 166, "X1": 126, "Y1": 166, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 106, "Yo": 166, "Zo": 166, "X1": 126, "Y1": 186, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 106, "Yo": 186, "Zo": 166, "X1": 126, "Y1": 206, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 106, "Yo": 206, "Zo": 166, "X1": 126, "Y1": 226, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 126, "Yo": 126, "Zo": 166, "X1": 146, "Y1": 146, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 126, "Yo": 146, "Zo": 166, "X1": 146, "Y1": 166, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 126, "Yo": 166, "Zo": 166, "X1": 146, "Y1": 186, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 126, "Yo": 186, "Zo": 166, "X1": 146, "Y1": 206, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 126, "Yo": 206, "Zo": 166, "X1": 146, "Y1": 226, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 146, "Yo": 126, "Zo": 166, "X1": 166, "Y1": 146, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 146, "Yo": 146, "Zo": 166, "X1": 166, "Y1": 166, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 146, "Yo": 166, "Zo": 166, "X1": 166, "Y1": 186, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 146, "Yo": 186, "Zo": 166, "X1": 166, "Y1": 206, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 146, "Yo": 206, "Zo": 166, "X1": 166, "Y1": 226, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 166, "Yo": 126, "Zo": 166, "X1": 186, "Y1": 146, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 166, "Yo": 146, "Zo": 166, "X1": 186, "Y1": 166, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 166, "Yo": 166, "Zo": 166, "X1": 186, "Y1": 186, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 166, "Yo": 186, "Zo": 166, "X1": 186, "Y1": 206, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 166, "Yo": 206, "Zo": 166, "X1": 186, "Y1": 226, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 186, "Yo": 126, "Zo": 166, "X1": 206, "Y1": 146, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 186, "Yo": 146, "Zo": 166, "X1": 206, "Y1": 166, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 186, "Yo": 166, "Zo": 166, "X1": 206, "Y1": 186, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 186, "Yo": 186, "Zo": 166, "X1": 206, "Y1": 206, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 186, "Yo": 206, "Zo": 166, "X1": 206, "Y1": 226, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 206, "Yo": 126, "Zo": 166, "X1": 226, "Y1": 146, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 206, "Yo": 146, "Zo": 166, "X1": 226, "Y1": 166, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 206, "Yo": 166, "Zo": 166, "X1": 226, "Y1": 186, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 206, "Yo": 186, "Zo": 166, "X1": 226, "Y1": 206, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 206, "Yo": 206, "Zo": 166, "X1": 226, "Y1": 226, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 226, "Yo": 126, "Zo": 166, "X1": 246, "Y1": 146, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 226, "Yo": 146, "Zo": 166, "X1": 246, "Y1": 166, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 3, "Xo": 226, "Yo": 166, "Zo": 166, "X1": 246, "Y1": 186, "Z1": 196, "Color": "#ffb350", "Name": "C" }, { "LineNumber": 4, "Xo": 6, "Yo": 226, "Zo": 6, "X1": 106, "Y1": 281, "Z1": 86, "Color": "#00acc1", "Name": "B" }, { "LineNumber": 4, "Xo": 6, "Yo": 226, "Zo": 86, "X1": 106, "Y1": 281, "Z1": 166, "Color": "#00acc1", "Name": "B" }, { "LineNumber": 4, "Xo": 106, "Yo": 226, "Zo": 6, "X1": 206, "Y1": 281, "Z1": 86, "Color": "#00acc1", "Name": "B" }], "Container": { "Name": "Name1", "Length": 306, "Width": 286 }, "ExecutionPercent": 100.0 }


              selectBoxesOfALine = (lineNumber) ->
                  if lineNumber < 0 or lineNumber > data.Boxes[data.Boxes.length-1].LineNumber
                    data
                  else
                      containerData = data.Container
                      sortedData = (box for box in data.Boxes when box.LineNumber == lineNumber)
                      toRenderData = {
                        Boxes : sortedData
                        Container : containerData
                      }
                      toRenderData

              render = (lineNumber) ->
                  dataToRender = selectBoxesOfALine(lineNumber)
                  create3DModel(dataToRender)
                  return

              create3DModel = (data) ->
                      # creating model

                      model = seen.Models.default()

                      model.add(seen.Shapes.rectangle(seen.P(0, 0, 0), seen.P(data.Container.Width, data.Container.Length, 2)))

                      for item in data.Boxes
                          model.add(seen.Shapes.rectangle(seen.P(item.Xo + 3, item.Yo + 3, item.Zo + 3), seen.P(item.X1 - 3, item.Y1 - 3, item.Z1 - 3))
                              .fill(item.Color))

                      # creating scenes

                      scenes = [0...5].map (i) -> new seen.Scene
                          model     : model
                          viewport  : seen.Viewports.center(dims[i]...)

                      scenes[0].camera.scale(0.9) # perspective view

                      scenes[1].camera.scale(0.7) # top view

                      scenes[2].camera.scale(0.7) # front view
                      scenes[2].camera.rotx(-Math.PI/2)
                      scenes[2].camera.roty(-Math.PI)

                      scenes[3].camera.scale(0.9) # back view
                      scenes[3].camera.rotx(-Math.PI/2)

                      scenes[4].camera.scale(0.9) # side view
                      scenes[4].camera.rotx(-Math.PI/2)
                      scenes[4].camera.roty(Math.PI/2)

                      # rendering scenes

                      contexts = scenes.map (scene,i) -> seen.Context("seen-canvas-#{i}", scene)
                      mainContext = seen.Context('seen-canvas-0', scenes[0]).render()

                      renderAll = -> context.render() for context in contexts
                      renderAll()

                      # dragging

                      dragger = new seen.Drag('seen-canvas-0')
                      dragger.on('drag.rotate', (e) ->
                          xform = seen.Quaternion.xyToTransform(e.offsetRelative...)
                          model.transform(xform)
                          mainContext.render()
                      )

              getData()
              render(-1)

              $("#linesTotalQuantity").text(data.Boxes[data.Boxes.length-1].LineNumber)

              $(document).on "click", "#resetRender",->
                $(".renderError").text("");
                render(-1)

              $(document).on "click", "#filterRender", ->
                $(".renderError").text("");
                lineNumber = parseInt($("#lineNumberInput").val().trim());
                if (lineNumber < 0 || lineNumber > data.Boxes[data.Boxes.length-1].LineNumber)
                    $(".renderError").text("Invalid line number");
                else render(lineNumber)
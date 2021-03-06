// Created by: JFowler
// November 2018

var BLOCK_SIDE_PIXEL_COUNT = 20;
var MAP_WIDTH_IN_BLOCKS = 144;
var MAP_HEIGHT_IN_BLOCKS = 88;
var GRID_BORDER_PIXEL_COUNT = (BLOCK_SIDE_PIXEL_COUNT / 10);

var road_objects = [];
var farm_objects = [];
var city_objects = [];
var unit_objects = [];
var wonder_objects = [];

var canvas = document.getElementById("mapCanvas");
var ctx = canvas.getContext("2d");
var mapID = document.getElementById("mapImage");

// Use JS onload instead of JQuery to ensure all content is loaded before attempting to
// use the various components. 

window.onload = function() 
{
    // Add common tab code to html

    generateMetaContent($("#metaContent"));

    // Initialize Canvas

    renderCanvasImage();

    // Initialize tabs

    $('.tabs').tabs();

    // Initialize selects

    $('select').formSelect();

    // Initialize modal

    $('.modal').modal();

    // Register map click handler

    $('#mapCanvas').click( function (e)
    {
        mapCanvasClickHandler(e.offsetX, e.offsetY);
    });

    // Register create button handler

    $('#btn_create').click( function (e)
    {
        createButtonClickHandler();
    });
}

function generateMetaContent(parentID)
{
    var contentTab = '';

    // General Meta Content

    contentTab += '                                                 \
    <div class="row">                                               \
        <div class="input-field col s6">                            \
            <select class="metaPlayerCount">                        \
                <option value="4">4</option>                        \
                <option value="3">3</option>                        \
                <option value="2">2</option>                        \
            </select>                                               \
            <label>Number of Players:</label>                       \
        </div>                                                      \
        <div class="input-field col s6">                            \
            <select class="metaAnarchyEnabled">                     \
                <option value="true">true</option>                  \
                <option value="false">false</option>                \
            </select>                                               \
            <label>Enable Anarchy? (Barbarians):</label>            \
        </div>                                                      \
    </div>                                                          \
    ';

    // Player and Nature tabs

    contentTab += '                                                 \
    <ul class="tabs tabs-fixed-width z-depth-1 player-tabs">        \
        <li class="tab">                                            \
            <a href="#tabPlayer1" class="active">1</a>              \
        </li>                                                       \
        <li class="tab">                                            \
            <a href="#tabPlayer2">2</a>                             \
        </li>                                                       \
        <li class="tab">                                            \
            <a href="#tabPlayer3">3</a>                             \
        </li>                                                       \
        <li class="tab">                                            \
            <a href="#tabPlayer4">4</a>                             \
        </li>                                                       \
        <li class="tab">                                            \
            <a href="#tabPlayer5">Anarchy</a>                       \
        </li>                                                       \
        <li class="tab">                                            \
            <a href="#tabNature">Nature</a>                         \
        </li>                                                       \
    </ul>                                                           \
    ';

    // Fill content of player 1-5 tabs

    for (var i = 1; i <= 5; i++)
    {
        // Get the current time and wait until the next milisecond
        // to ensure that subsequent calls do not use the same time
        // id. The time value is used to randomize various id's.

        var time = Date.now();
        while (Date.now() == time)
        {
            ;
        }

        var playerTab = '';

        playerTab = '                                               \
        <br>                                                        \
        <div class="row">                                           \
            <div class="input-field col s6">                        \
                <input id="playerName_' + time + '"                 \
                class="playerName" type="text" maxlength="64">      \
                <label for="playerName_' + time + '">               \
                    Player Name:                                    \
                </label>                                            \
            </div>                                                  \
            <div class="input-field col s6">                        \
                <select class="playerType">' +
                    ((5 == i) ? 
                    '<option value="3">Anarchy</option>'
                    :
                    '<option value="1">Human</option>               \
                    <option value="2">Computer</option>') + 
                '</select>                                          \
                <label>Player Type:</label>                         \
            </div>                                                  \
            <div class="input-field col s6">                        \
                <select class="playerCivilization">' +
                    ((5 == i) ?
                    '<option value="6">Barbarian</option>'
                    :
                    '<option value="0">Greek</option>               \
                    <option value="1">Egyptian</option>             \
                    <option value="2">Indian</option>               \
                    <option value="3">Mesopotamian</option>         \
                    <option value="4">Chinese</option>              \
                    <option value="5">Celt</option>                 \
                    <option value="6">Barbarian</option>') +
                '</select>                                          \
                <label>Nation:</label>                              \
            </div>                                                  \
            <div class="input-field col s6">                        \
                <select class="playerMapExplored">                  \
                    <option value="true">yes</option>               \
                    <option value="false">no</option>               \
                </select>                                           \
                <label>Map Fully Explored:</label>                  \
            </div>                                                  \
        </div>                                                      \
                                                                    \
        <p>Select and configure object.                             \
        Then click on map to place.</p>                             \
                                                                    \
        <ul class="tabs tabs-fixed-width z-depth-1 obj-tabs">       \
            <li class="tab">                                        \
                <a href="#tabCity_' + time + '" class="active">     \
                    city                                            \
                </a>                                                \
            </li>                                                   \
            <li class="tab">                                        \
                <a href="#tabUnit_' + time + '">                    \
                    unit                                            \
                </a>                                                \
            </li>                                                   \
        </ul>                                                       \
                                                                    \
        <div id="tabCity_' + time + '">                             \
            <div class="row">                                       \
                <div class="input-field col s12">                   \
                    <input id="cityName_' + time + '"               \
                    class="cityName" type="text" maxlength="64">    \
                    <label for="cityName_' + time + '">             \
                        City Name:                                  \
                    </label>                                        \
                </div>                                              \
                                                                    \
                <p>All Buildings</p>                                \
                <div class="switch">                                \
                    <label>                                         \
                        false                                       \
                        <input type="checkbox"                      \
                        class="cityAllBuildings">                   \
                        <span class="lever"></span>                 \
                        true                                        \
                    </label>                                        \
                </div>                                              \
                                                                    \
                <p>All Knowledge</p>                                \
                <div class="switch">                                \
                    <label>                                         \
                        false                                       \
                        <input type="checkbox"                      \
                        class="cityAllKnowledge">                   \
                        <span class="lever"></span>                 \
                        true                                        \
                    </label>                                        \
                </div>                                              \
            </div>                                                  \
        </div>                                                      \
        <div id="tabUnit_' + time + '">                             \
            <br>                                                    \
            <div class="row">                                       \
                <div class="input-field col s6">                    \
                    <select class="unitType">                       \
                    <option value="500a">Light Missle</option>      \
                    <option value="6006">Heavy Missle</option>      \
                    <option value="000a">Light Infantry</option>    \
                    <option value="10O8">Medium Infantry</option>   \
                    <option value="21O6">Heavy Infantry</option>    \
                    <option value="30Oe">Light Calvary</option>     \
                    <option value="40OC">Heavy Calvary</option>     \
                    <option value="d207">Jungle Raider</option>     \
                    <option value="c30c">Desert Raider</option>     \
                    <option value="70Og">Light Ship</option>        \
                    <option value="81Ok">Heavy Ship</option>        \
                    <option value="91Oa">Philosopher</option>       \
                    <option value="b0Oa">Merchant</option>          \
                    <option value="a0Oa">Settler</option>           \
                    </select>                                       \
                    <label>Unit Type:</label>                       \
                </div>                                              \
                <div class="input-field col s6">                    \
                    <select class="unitCount">                      \
                    <option value="1">1</option>                    \
                    <option value="2">2</option>                    \
                    <option value="3">3</option>                    \
                    <option value="4">4</option>                    \
                    <option value="5">5</option>                    \
                    <option value="10">10</option>                  \
                    <option value="20">20</option>                  \
                    <option value="25">25</option>                  \
                    <option value="27">27</option>                  \
                    </select>                                       \
                    <label>Unit Count:</label>                      \
                </div>                                              \
            </div>                                                  \
        </div>                                                      \
        ';

        contentTab += '                                             \
            <div id="tabPlayer' + i + '">' + playerTab + '          \
            </div>                                                  \
        ';
    }

    // Nature Tab

    contentTab += '                                                 \
    <div id="tabNature">                                            \
        <p>Select and configure object.                             \
        Then click on map to place.</p>                             \
                                                                    \
        <ul class="tabs tabs-fixed-width z-depth-1 obj-tabs">       \
            <li class="tab">                                        \
                <a href="#tabRoad_' + time + '" class="active">     \
                    road                                            \
                </a>                                                \
            </li>                                                   \
            <li class="tab">                                        \
                <a href="#tabFarm_' + time + '">                    \
                    farm                                            \
                </a>                                                \
            </li>                                                   \
            <li class="tab">                                        \
                <a href="#tabWonder_' + time + '">                  \
                    wonder                                          \
                </a>                                                \
            </li>                                                   \
        </ul>                                                       \
        <br>                                                        \
        <div id="tabWonder_' + time + '">                           \
            <br>                                                    \
            <div class="row">                                       \
                <div class="input-field col s6">                    \
                    <select class="wonderCivilization">             \
                    <option value="0">Greek</option>                \
                    <option value="1">Egyptian</option>             \
                    <option value="2">Indian</option>               \
                    <option value="3">Mesopotamian</option>         \
                    <option value="4">Chinese</option>              \
                    <option value="5">Celt</option>                 \
                    </select>                                       \
                    <label>Civilization:</label>                    \
                </div>                                              \
            </div>                                                  \
        </div>                                                      \
    </div>                                                          \
    ';

    $(parentID).prepend(contentTab);
}

function renderCanvasImage()
{
    var coords;

    // Draw map image

    ctx.drawImage(mapID, 0, 0);

    // Draw cities - Square in middle

    for (var i = 0; i < city_objects.length; i++)
    {
        coords = city_objects[i].coords;
        ctx.fillStyle = getObjectColor(city_objects[i].player).city;
        ctx.fillRect(coords.cornerX + GRID_BORDER_PIXEL_COUNT + 4, coords.cornerY + GRID_BORDER_PIXEL_COUNT + 4, 10, 10);
    }

    // Draw units - Square in middle - Little smaller than cities

    for (var i = 0; i < unit_objects.length; i++)
    {
        coords = unit_objects[i].coords;
        ctx.fillStyle = getObjectColor(unit_objects[i].player).unit;
        ctx.fillRect(coords.cornerX + GRID_BORDER_PIXEL_COUNT + 5, coords.cornerY + GRID_BORDER_PIXEL_COUNT + 5, 8, 8);
    }

    // Draw wonders - Square in middle

    for (var i = 0; i < wonder_objects.length; i++)
    {
        coords = wonder_objects[i].coords;
        ctx.fillStyle = getObjectColor(wonder_objects[i].player).wonder;
        ctx.fillRect(coords.cornerX + GRID_BORDER_PIXEL_COUNT + 4, coords.cornerY + GRID_BORDER_PIXEL_COUNT + 4, 10, 10);
    }

    // Draw farms - Outer hallow square

    for (var i = 0; i < farm_objects.length; i++)
    {
        ctx.fillStyle = getObjectColor(farm_objects[i].player).farm;
        coords = farm_objects[i].coords;
        ctx.fillRect(coords.cornerX + GRID_BORDER_PIXEL_COUNT, coords.cornerY + GRID_BORDER_PIXEL_COUNT, BLOCK_SIDE_PIXEL_COUNT - GRID_BORDER_PIXEL_COUNT, 4);
        ctx.fillRect(coords.cornerX + GRID_BORDER_PIXEL_COUNT, coords.cornerY + GRID_BORDER_PIXEL_COUNT, 4, BLOCK_SIDE_PIXEL_COUNT - GRID_BORDER_PIXEL_COUNT);
        ctx.fillRect(coords.cornerX + (BLOCK_SIDE_PIXEL_COUNT - 4), coords.cornerY + GRID_BORDER_PIXEL_COUNT, 4, BLOCK_SIDE_PIXEL_COUNT - GRID_BORDER_PIXEL_COUNT);
        ctx.fillRect(coords.cornerX + GRID_BORDER_PIXEL_COUNT, coords.cornerY + (BLOCK_SIDE_PIXEL_COUNT - 4), BLOCK_SIDE_PIXEL_COUNT - GRID_BORDER_PIXEL_COUNT, 4);
    }

    // Draw roads - Plus sign

    for (var i = 0; i < road_objects.length; i++)
    {
        ctx.fillStyle = getObjectColor(road_objects[i].player).road;
        coords = road_objects[i].coords;
        ctx.fillRect(coords.cornerX + GRID_BORDER_PIXEL_COUNT + 8, coords.cornerY + GRID_BORDER_PIXEL_COUNT, 2, BLOCK_SIDE_PIXEL_COUNT - GRID_BORDER_PIXEL_COUNT);
        ctx.fillRect(coords.cornerX + GRID_BORDER_PIXEL_COUNT, coords.cornerY + GRID_BORDER_PIXEL_COUNT + 8, BLOCK_SIDE_PIXEL_COUNT - GRID_BORDER_PIXEL_COUNT, 2);
    }
}

function indexOfObject(objArray, x, y)
{
    var index = -1;

    for (var i = 0; i < objArray.length; i++)
    {
        if ((objArray[i].coords.x == x) && (objArray[i].coords.y == y))
        {
            index = i;
            break;
        }
    }

    return index;
}

function getObjectColor(playerID)
{
    var color = {};

    switch (playerID)
    {
        case 1:
        case '1':
            color.city = "#FF0000";
            color.unit = "#FF8888";
            break;

        case 2:
        case '2':
            color.city = "#145A32";
            color.unit = "#00FF00";
            break;

        case 3:
        case '3':
            color.city = "#0000FF";
            color.unit = "#5DADE2";
            break;

        case 4:
        case '4':
            color.city = "#4A235A";
            color.unit = "#A569BD";
            break;

        case 5:
        case '5':
        case 'Anarchy':
            color.city = "#000000";
            color.unit = "#444444";
            break;

        case 6:
        case '6':
        case 'Nature':
            color.road = "#6E2C00";
            color.farm = "#B7950B";
            color.wonder = "#6E2C00";
            break;

        default:
            break;
    }

    return color;
}

function getCityInfo(playerNum)
{
    var cityInfo = {};

    // City name

    cityInfo.name = $("#tabPlayer" + playerNum + " .cityName").val();
    if ("" == cityInfo.name)
    {
        cityInfo.name = "Outpost";
    }

    // City developed

    cityInfo.developed = $("#tabPlayer" + playerNum + " .cityAllBuildings").prop("checked");

    // City knowledge

    cityInfo.knowledge = $("#tabPlayer" + playerNum + " .cityAllKnowledge").prop("checked");

    return cityInfo;
}

function getUnitInfo(playerNum)
{
    var unitInfo = {};

    // Unit type

    unitInfo.type = $("#tabPlayer" + playerNum + " .unitType").val().trim();

    // Unit count

    unitInfo.count = $("#tabPlayer" + playerNum + " .unitCount").val().trim();

    return unitInfo;
}

function getWonderInfo()
{
    var wonderInfo = {};

    wonderInfo.civilization = $("#tabNature .wonderCivilization").val().trim();

    return wonderInfo;
}

function convertOffsetsToCoords(x, y)
{
    var coords = {
        x:0,        // X block number (not pixel)
        y:0,        // Y block number (not pixel)
        xy:0,       // Absolute block number
        cornerX:0,  // X Pixel location of top-left corner
        cornerY:0   // Y Pixel location of top-left corner
    };

    coords.x = parseInt(x / BLOCK_SIDE_PIXEL_COUNT);
    coords.y = parseInt(y / BLOCK_SIDE_PIXEL_COUNT);

    coords.xy = (coords.y * MAP_WIDTH_IN_BLOCKS) + coords.x;

    coords.cornerX = coords.x * BLOCK_SIDE_PIXEL_COUNT;
    coords.cornerY = coords.y * BLOCK_SIDE_PIXEL_COUNT;

    return coords;
}

function mapCanvasClickHandler(x, y)
{
    var objMeta = { coords:{}, player:"", objType:"" };

    objMeta.coords = convertOffsetsToCoords(parseInt(x), parseInt(y));
    objMeta.player = $(".player-tabs .active").html().trim();

    // Set Anarchy as player 5

    if ("Anarchy" == objMeta.player)
    {
        objMeta.player = '5';
    }

    // Get Object Type

    if ("Nature" == objMeta.player)
    {
        objMeta.objType = $("#tabNature .obj-tabs .active").html().trim();
    }
    else
    {
        objMeta.objType = $("#tabPlayer" + objMeta.player + " .obj-tabs .active").html().trim();
    }

    // Depending upon object type, if the block is already taken, erase
    // the current block object, else, add new object. Roads and farms
    // can be stacked on each other and on cities and units. 

    var objArrayPtr = null;
    var index = -1;

    if ("road" == objMeta.objType)
    {
        objArrayPtr = road_objects;
    }
    else if ("farm" == objMeta.objType)
    {
        objArrayPtr = farm_objects;
    } 
    else if ("wonder" == objMeta.objType)
    {
        objArrayPtr = wonder_objects;
        objMeta.wonder = getWonderInfo();
    } 
    else if ("unit" == objMeta.objType)
    {
        objArrayPtr = unit_objects;
        objMeta.unit = getUnitInfo(objMeta.player);
    }
    else if ("city" == objMeta.objType)
    {
        objArrayPtr = city_objects;
        objMeta.city = getCityInfo(objMeta.player);
    }
    else
    {
        // Illegal type... 
        return;
    }

    var index = indexOfObject(objArrayPtr, objMeta.coords.x, objMeta.coords.y);

    if (-1 == index)
    {
        objArrayPtr.push(objMeta);
    }
    else
    {
        objArrayPtr.splice(index, 1);
    }

    renderCanvasImage();
}

function createButtonClickHandler()
{
    var playerCount = $(".metaPlayerCount").val().trim();
    var anarchyEnabled = $(".metaAnarchyEnabled").val().trim();

    console.log(anarchyEnabled);

    var playerInfo = [ {}, {}, {}, {}, {} ];

    for (var i = 0; i < 5; i++)
    {
        // Player Name

        playerInfo[i].name = $("#tabPlayer" + (i + 1) + " .playerName").val().trim();
        if ("" == playerInfo[i].name)
        {
            playerInfo[i].name = "Player " + (i + 1);
        }

        // Player Type

        playerInfo[i].type = $("#tabPlayer" + (i + 1) + " .playerType").val().trim();

        // Player Nation

        playerInfo[i].nation = $("#tabPlayer" + (i + 1) + " .playerCivilization").val().trim();

        // Map Explored

        playerInfo[i].exloredMap = $("#tabPlayer" + (i + 1) + " .playerMapExplored").val().trim();
    }

    var fileStr = '';
    var tempStr = ['', '', '', '', ''];

    var tempMap = '';
    var indexS;

    fileStr +=  
        '[Game]\r\n'+
        'Map=Worldfin.map\r\n' +
        'WhoseTurn=0\r\n' +
        'Scale=100\r\n' + 
        'Round=0\r\n';

    for (var i = 0; i < 5; i++)
    {
        fileStr += 
            'Player=' + i + ',"' + playerInfo[i].name + '",' + 
            playerInfo[i].nation + ',' + playerInfo[i].type + ',' + 
            playerInfo[i].nation + '\r\n';
    }

    for (var i = 0; i < 5; i++)
    {
        fileStr += 'ID=' + i + ',"R&R Player"\r\n';
    }
                
    fileStr += 
        'lost=0,0,' + 
        ((3 <= playerCount) ? '0' : '1') + ',' + 
        ((4 <= playerCount) ? '0' : '1') + ',' + 
        (("true" == anarchyEnabled) ? '0' : '1') + '\r\n';

    for (var i = 0; i < 5; i++)
    {
        fileStr += '[Army' + i + ']\r\n';

        for (var j = 0; j < unit_objects.length; j++)
        {
            var objPtr = unit_objects[j];
            var player = objPtr.player;

            if (!(parseInt(player) == (i + 1)) || 
                !(("500a" == objPtr.unit.type) || // Light Missle
                  ("6006" == objPtr.unit.type) || // Heavy Missle
                  ("000a" == objPtr.unit.type) || // Light Infantry
                  ("10O8" == objPtr.unit.type) || // Medium Infantry
                  ("21O6" == objPtr.unit.type) || // Heavy Infantry
                  ("30Oe" == objPtr.unit.type) || // Light Calvary
                  ("40OC" == objPtr.unit.type) || // Heavy Calvary
                  ("d207" == objPtr.unit.type) || // Jungle Raider
                  ("c30c" == objPtr.unit.type))) // Desert Raider
            {
                continue;
            }

            fileStr += objPtr.coords.xy + '=';

            for (var k = 0; k < objPtr.unit.count; k++)
            {
                fileStr += objPtr.unit.type;
            }

            fileStr += ',100,100,0\r\n';
        }
    }

    for (var i = 0; i < 5; i++)
    {
        fileStr += '[Philosopher' + i + ']\r\n';

        for (var j = 0; j < unit_objects.length; j++)
        {
            var objPtr = unit_objects[j];
            var player = objPtr.player;

            if (!(parseInt(player) == (i + 1)) || 
                !(("91Oa" == objPtr.unit.type))) // Philosopher
            {
                continue;
            }

            for (var k = 0; k < objPtr.unit.count; k++)
            {
                fileStr += objPtr.coords.xy + '=' + objPtr.unit.type +
                           ',1000000,1000000,1000000,1000000,1000000,"test phil"\r\n';
            }
        }
    }   

    for (var i = 0; i < 5; i++)
    {
        fileStr += '[Settler' + i + ']\r\n';

        for (var j = 0; j < unit_objects.length; j++)
        {
            var objPtr = unit_objects[j];
            var player = objPtr.player;

            if (!(parseInt(player) == (i + 1)) || 
                !(("b0Oa" == objPtr.unit.type))) // Settler
            {
                continue;
            }

            for (var k = 0; k < objPtr.unit.count; k++)
            {
                fileStr += objPtr.coords.xy + '=' + objPtr.unit.type +
                           ',1000000,1000000,1000000,1000000,1000000\r\n';
            }
        }
    }   

    for (var i = 0; i < 5; i++)
    {
        fileStr += '[Merchant' + i + ']\r\n';

        for (var j = 0; j < unit_objects.length; j++)
        {
            var objPtr = unit_objects[j];
            var player = objPtr.player;

            if (!(parseInt(player) == (i + 1)) || 
                !(("a0Oa" == objPtr.unit.type))) // Philosopher
            {
                continue;
            }

            for (var k = 0; k < objPtr.unit.count; k++)
            {
                fileStr += objPtr.coords.xy + '=' + objPtr.unit.type + ',1,1\r\n';
            }
        }
    }   

    for (var i = 0; i < 5; i++)
    {
        fileStr += '[Ship' + i + ']\r\n';

        for (var j = 0; j < unit_objects.length; j++)
        {
            var objPtr = unit_objects[j];
            var player = objPtr.player;

            if (!(parseInt(player) == (i + 1)) || 
                !(("70Og" == objPtr.unit.type) || // Ligh Ship
                  ("81Ok" == objPtr.unit.type)))  // Heavy Ship
            {
                continue;
            }

            for (var k = 0; k < objPtr.unit.count; k++)
            {
                fileStr += objPtr.coords.xy + '=' + objPtr.unit.type + ',"test ship"\r\n';
            }
        }
    }   

    for (var i = 0; i < 5; i++)
    {
        fileStr += '[Orders' + i + ']\r\n';
    }

    for (var i = 0; i < city_objects.length; i++)
    {
        var objPtr = city_objects[i];
        var player = objPtr.player;

        tempStr[(player - 1)] += 
            '[City' + (player - 1) + '@' + objPtr.coords.xy + ']\r\n' + 
            'city="' + objPtr.city.name + '",' + ((objPtr.city.developed) ? '3' : '1') + '\r\n' +
            'pop=' + ((objPtr.city.developed) ? '2000' : '300') + ',0,0,z1a3,aaaaa,OO000\r\n' +
            'know=' + ((objPtr.city.knowledge) ? '1000000,1000000,1000000,1000000,1000000' : '0,0,0,0,0') + '\r\n' +
            'prod=0,0,0\r\n' + 
            'building=' + 

            ((objPtr.city.developed) ? '3' : '1') + (player - 1) + // TownHall
            ((objPtr.city.developed) ? '3' : '0') + (player - 1) + // Workshop
            ((objPtr.city.developed) ? '3' : '0') + (player - 1) + // Market
            ((objPtr.city.developed) ? '3' : '1') + (player - 1) + // Academy
            ((objPtr.city.developed) ? '3' : '0') + (player - 1) + // Temple
            ((objPtr.city.developed) ? '3' : '0') + (player - 1) + // Library
            ((objPtr.city.developed) ? '3' : '0') + (player - 1) + // Barracks
            ((objPtr.city.developed) ? '3' : '0') + (player - 1) + // Warehouse
            ((objPtr.city.developed) ? '3' : '0') + (player - 1) + // Blacksmith
            ((objPtr.city.developed) ? '3' : '0') + (player - 1) + // Wall

            '\r\n' +
            'trade=pppp,0,' +

            (('1' == player) ? 'B0000' : '') +
            (('2' == player) ? '0B000' : '') +
            (('3' == player) ? '00B00' : '') +
            (('4' == player) ? '000B0' : '') +
            (('5' == player) ? '0000B' : '') +

            '\r\n' + 
            'goods=0,0,0,0,0,0,0,0,0\r\n' + 
            'orders=0,0,0\r\n';
    }

    for (var i = 0; i < 5; i++)
    {
        fileStr += tempStr[i];
        tempStr[i] = '';
    }

    for (var i = 0; i < 5; i++)
    {
        fileStr += '[Spotted' + i + ']\r\n';

        if ('true' == playerInfo[i].exloredMap)
        {
            for (var j = 0; j < MAP_HEIGHT_IN_BLOCKS; j++)
            {
                fileStr += j + '=ffffffffffffffffffffffffffffffffffff\r\n';
            }
        }

    }

    for (var i = 0; i < 5; i++)
    {
        fileStr += '[Diplomacy' + i + ']\r\n';

        for (var j = 0; j < 5; j++)
        {
            if (i != j)
            {
                fileStr += 'dip=' + j + ',51,51,0,0\r\n';
            }
        }
    }

    for (var i = 0; i < 5; i++)
    {
        fileStr += '[CombatStats' + i + ']\r\n';

        for (var j = 0; j < 5; j++)
        {
            if (i != j)
            {
                fileStr += 'stats=' + j + ',0,0,0,0,0,0,0,0,0,0\r\n';
            }
        }
    }

    for (var i = 0; i < 5; i++)
    {
        fileStr += '[EventStatus' + i + ']\r\n';
    }

    fileStr += '[Road]\r\n';
    tempMap = '';

    for (var i = 0; i < MAP_HEIGHT_IN_BLOCKS; i++)
    {
        tempMap += i + '=000000000000000000000000000000000000\r\n';
    }

    for (var i = 0; i < road_objects.length; i++)
    {
        var x = road_objects[i].coords.x;
        var y = road_objects[i].coords.y;

        indexS = tempMap.indexOf(y + '=');
        indexS = tempMap.indexOf('=', indexS) + 1;
        indexS += parseInt(x / 4);

        var newValue = (parseInt(tempMap[indexS], 16) +  Math.pow(2, parseInt(x % 4))).toString(16);
        tempMap = (tempMap.substring(0, indexS) + newValue + tempMap.substring(indexS + 1));
    }

    fileStr += tempMap;

    fileStr += '[Cultivation]\r\n';
    tempMap = '';

    for (var i = 0; i < MAP_HEIGHT_IN_BLOCKS; i++)
    {
        tempMap += i + '=000000000000000000000000000000000000\r\n';
    }

    for (var i = 0; i < farm_objects.length; i++)
    {
        var x = farm_objects[i].coords.x;
        var y = farm_objects[i].coords.y;

        indexS = tempMap.indexOf(y + '=');
        indexS = tempMap.indexOf('=', indexS) + 1;
        indexS += parseInt(x / 4);

        var newValue = (parseInt(tempMap[indexS], 16) +  Math.pow(2, parseInt(x % 4))).toString(16);
        tempMap = (tempMap.substring(0, indexS) + newValue + tempMap.substring(indexS + 1));
    }

    fileStr += tempMap;

    fileStr += '[Wonders]\r\n'
    for (var i = 0; i < wonder_objects.length; i++)
    {
        var objPtr = wonder_objects[i];
        fileStr += 'w=' + objPtr.wonder.civilization + ',0,' + objPtr.coords.xy + ',0\r\n';
    }
        
    fileStr += '[End]\r\n\r\n';

    //console.log(fileStr);

    var base64Str = window.btoa(fileStr);

    $("#downloadModal .modal-content").html('All Done!<br>Get file here:<br><a href="data:application/octet-stream;charset=utf-16le;base64,' + base64Str + '">RiseAndRule.sav</a>');
    $('#downloadModal').modal('open'); 
}

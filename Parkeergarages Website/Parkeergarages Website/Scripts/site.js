// Google Maps //
var map;

$(document).ready(function () {
    if (controller == 'garage' && action == 'overview') {
        overviewMap();
    }
    else if (controller == 'garage' && action == 'details') {
        initDetails();
        detailsMap();
        initGraphs();
    }
});

function overviewMap() {
    var center_nl = { lat: 52.277198, lng: 5.243578 };
    map = new google.maps.Map(document.getElementById('overview_map'), {
        zoom: 8,
        center: center_nl
    });

    var markers = [];
    for (var i = 0; i < garages.length; i++) {
        var garage_id               = garages[i].garage_id;
        var garage_name             = garages[i].name;
        var garage_longitude        = garages[i].latitude;
        var garage_latitude         = garages[i].longitude;
        var garage_aantal_plekken   = garages[i].aantal_plekken;
        var garage_loc = { lat: garage_latitude, lng: garage_longitude };

        markers[i] = {
            infowindow      : null,
            marker          : null,
            contentString   : null
        };

        markers[i].contentString = '<div id="content">' +
            '<div id="siteNotice">' +
            '</div>' +
            '<h1 id="firstHeading" class="firstHeading">' + garage_name + '</h1>' +
            '<div id="bodyContent">' +
            'Garage details kort.</p>' +
            '<p><a href="/garage/details?garage_id=' + garage_id + '"> Meer details </a> </p>' +
            '</div>' +
            '</div>';

        markers[i].infowindow = new google.maps.InfoWindow({
            content: markers[i].contentString,
            maxWidth: 350
        });

        markers[i].marker = new google.maps.Marker({
            position: garage_loc,
            map: map,
            title: garage_name,
            i: i
        });

        markers[i].marker.addListener('click', function () {
            for (var j = 0; j < markers.length; j++) {
                markers[j].infowindow.close();
            }
            markers[this.i].infowindow.open(map, markers[this.i].marker);
        });
    }
}

function detailsMap() {
    var garage_loc = { lat: garage_info.longitude, lng: garage_info.latitude };
    var garage_naam = garage_info.name;

    map = new google.maps.Map(document.getElementById('details_map'), {
        zoom: 17,
        center: garage_loc,
        mapTypeId: google.maps.MapTypeId.SATELLITE,
        disableDefaultUI: true,
        draggable: false,
        zoomControl: false,
        scrollwheel: false,
        disableDoubleClickZoom: true
    });

    var marker = new google.maps.Marker({
        position: garage_loc,
        map: map,
        title: garage_naam
    });
}

function initDetails() {
    if (garage_info.feiten[0].open && !garage_info.feiten[0].full) {
        var ratio = (garage_info.feiten[0].vrije_plekken / garage_info.aantal_plekken) * 100;

        if (ratio >= 80) {
            document.getElementById('infobox').style.backgroundColor = '#00ff00';
        } else if (ratio >= 60) {
            document.getElementById('infobox').style.backgroundColor = '#ccff66';
            document.getElementById('infobox').style.color = 'black';
        } else if (ratio >= 40) {
            document.getElementById('infobox').style.backgroundColor = '#ffff66';
            document.getElementById('infobox').style.color = 'black';
        } else if (ratio >= 20) {
            document.getElementById('infobox').style.backgroundColor = '#ffcc66';
            document.getElementById('infobox').style.color = 'black';
        } else {
            document.getElementById('infobox').style.backgroundColor = '#ff6600';
            document.getElementById('infobox').style.color = 'black';
        }
    }
}

function initGraphs() {
    var x = [];
    var y = [];

    for (var i = 0; i < garage_info.feiten.length; i++) {
        y.push(garage_info.feiten[i].vrije_plekken);

        var datum = garage_info.feiten[i].datum;
        datum = datum.replace('T', ' ');
        datum = datum.slice(0, -1);
        x.push(datum);
    }

    var data = [{
        y: y,
        x: x,
        line: { width: 1 },
        uid: "40abaa"
    }];
    var layout = {
        title: 'Historie',
        yaxis: { title: "Aantal vrije parkeerplekken"},      
        xaxis: {
            title: "Datum en tijd",
            showgrid: true
            //tickformat: '%a %b-%e-%Y'
        },
        margin: {                           
            l: 50, b: 50, r: 0, t: 40
        }
    };

    Plotly.plot(document.getElementById('graphs'), data, layout);
}




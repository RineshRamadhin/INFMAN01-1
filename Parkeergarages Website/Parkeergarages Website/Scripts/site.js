// Google Maps //
var map;

$(document).ready(function () {

    if (controller == 'garage' && action == 'overview') {
        overviewMap();
    }
    else if (controller == 'garage' && action == 'details') {
        detailsMap();
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




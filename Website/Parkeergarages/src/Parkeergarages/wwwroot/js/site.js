// Write your Javascript code.

// Parkeergarages //

// Google Maps //

$(document).ready(function () {
    overviewMap();
    detailswMap();
});

var rotterdam = { lat: 51.909149, lng: 4.486323 };

function overviewMap() {
    var map = new google.maps.Map(document.getElementById('overview_map'), {
        zoom: 12,
        center: rotterdam
    });

    // foreach garage maak infowindow en marker
    var contentString = '<div id="content">'+
        '<div id="siteNotice">'+
        '</div>'+
        '<h1 id="firstHeading" class="firstHeading">Garage Naam</h1>'+
        '<div id="bodyContent">'+
        'Garage details kort.</p>'+
        '<p><a href="/garage/details?garage_id=sdfjfivjijijciji"> Meer details </a> </p>' +
        '</div>'+
        '</div>';

    var infowindow = new google.maps.InfoWindow({
        content: contentString,
        maxWidth: 350
    });

    var marker = new google.maps.Marker({
        position: rotterdam,
        map: map,
        title: 'Garage naam'
    });
    marker.addListener('click', function() {
        infowindow.open(map, marker);
    });
}




var garage = { lat: 51.914029, lng: 4.455418 };

function detailsMap() {
    var map = new google.maps.Map(document.getElementById('details_map'), {
        zoom: 17,
        center: garage
    });

    var marker = new google.maps.Marker({
        position: garage,
        map: map,
        title: 'Garage naam'
    });
}

var latitude = "";
var longitude = "";
var propertyName = "";
var address = "";

$(document).ready(function () {

    $('#lat').val(lat)
    $('#long').val(long)
    $('#propertyname').val(propertyname)
    $('#addr').val(addr)

    latitude = lat;
    longitude = long;
    propertyName = propertyname;
    address = addr;

    function initialize() {
            var mapProp = {
                center:new google.maps.LatLng(latitude, longitude),
                zoom:16,
                mapTypeId:google.maps.MapTypeId.ROADMAP
            };
            var map=new google.maps.Map(document.getElementById("googleMap"),mapProp);
            var location = new google.maps.LatLng(latitude, longitude);
            var contentString = '<h3>' + propertyName + '</h3>' + '<p>' + address + '</p>'
            var infoWindow = new google.maps.InfoWindow({
                content: contentString
            });
            var marker = new google.maps.Marker({
                position: location,
                title: propertyName,
                map: map,
                draggable: false,
                animation: google.maps.Animation.DROP 
            });

            google.maps.event.addListener(marker, 'click', function(){
                infoWindow.open(map, marker);
            });

        }
        google.maps.event.addDomListener(window, 'load', initialize);
});


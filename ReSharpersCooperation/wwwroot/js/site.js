// Write your JavaScript code.

//SEARCH JS BEGIN
$(document).ready(function (e) {
    $('.search-panel .dropdown-menu').find('a').click(function (e) {
        e.preventDefault();
        var param = $(this).attr("href").replace("#", "");
        var concept = $(this).text();
        $('.search-panel span#search_concept').text(concept);
        $('.input-group #search_param').val(param);
    });
});
//SEARCH JS END

//JQUERY VALIDATION FIX BEGIN
$.validator.methods.range = function (value, element, param) {
    var globalizedValue = value.replace(",", ".");
    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
},
 
$.validator.methods.number = function (value, element) {
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\,]\d+)?$/.test(value);
}

//JQUERY VALIDATION FIX END

//PAYMENT METHOD DETECTION BEGIN
function yesnoCheck(that) {
    if (that.value === "creditcard") {
        document.getElementById("ifYes").style.display = "block";
    } else {
        document.getElementById("ifYes").style.display = "none";
    }
}
//PAYMENT METHOD DETECTION END

//GOOGLE MAPS JS BEGIN

var maps = [];
var markers = [];
function initMap() {
    var $maps = $('.map');
    $.each($maps, function (i, value) {
        var spot = { lat: parseFloat($(value).attr('lat')), lng: parseFloat($(value).attr('lng')) };

        var mapDivId = $(value).attr('id');

        maps[mapDivId] = new google.maps.Map(document.getElementById(mapDivId), {
            zoom: 10,
            center: spot
        });
        markers[mapDivId] = new google.maps.Marker({
            position: spot,
            map: maps[mapDivId]
        });
    })
}

//GOOGLE MAPS JS END

$('#long').rules('remove');



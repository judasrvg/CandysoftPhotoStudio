//var map, marker;

//function initializeMap(lat, lng, zoom) {
//    try {
//        map = L.map('map').setView([lat, lng], zoom);

//        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
//            attribution: '© OpenStreetMap contributors'
//        }).addTo(map);

//        map.on('click', function (e) {
//            console.log('Map clicked');
//            e.originalEvent.stopPropagation();
//            e.originalEvent.preventDefault();
//            return false;
//        });

//        map.on('touch', function (e) {
//            console.log('Map touched');
//            e.originalEvent.stopPropagation();
//            e.originalEvent.preventDefault();
//            return false;
//        });
//    }
//    catch { }
    
//}

//function addMarker(lat, lng, title) {
//    if (marker) {
//        marker.setLatLng([lat, lng]).bindPopup(title).openPopup();
//    } else {
//        marker = L.marker([lat, lng]).addTo(map).bindPopup(title).openPopup();
//    }
//}
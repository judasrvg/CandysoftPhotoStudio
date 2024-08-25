var map, marker;

function initializeMap(lat, lng, zoom) {
    try {
        map = L.map('map').setView([lat, lng], zoom);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors'
        }).addTo(map);

        map.on('click', function (e) {
            var latlng = e.latlng;
            if (marker) {
                marker.setLatLng(latlng);
            } else {
                marker = L.marker(latlng).addTo(map);
            }
            DotNet.invokeMethodAsync('Tattoo.Management', 'MapClicked', latlng.lat, latlng.lng);
        });
        map.on('touch', function (e) {
            var latlng = e.latlng;
            if (marker) {
                marker.setLatLng(latlng);
            } else {
                marker = L.marker(latlng).addTo(map);
            }
            DotNet.invokeMethodAsync('Tattoo.Management', 'MapClicked', latlng.lat, latlng.lng);
        }); }
    catch { }
    
}

function addMarker(lat, lng, title) {
    if (marker) {
        marker.setLatLng([lat, lng]).bindPopup(title).openPopup();
    } else {
        marker = L.marker([lat, lng]).addTo(map).bindPopup(title).openPopup();
    }
}
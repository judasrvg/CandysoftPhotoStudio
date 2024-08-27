//const cacheName = 'my-app-cache-v1';

//self.addEventListener('fetch', event => {
//    event.respondWith(
//        caches.open(cacheName).then(cache => {
//            return cache.match(event.request).then(response => {
//                return response || fetch(event.request).then(response => {
//                    cache.put(event.request, response.clone());
//                    return response;
//                });
//            });
//        })
//    );
//});

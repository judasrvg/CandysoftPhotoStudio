//// Cache names
//const CACHE_NAME = 'v1';

//// Files to cache
//const FILES_TO_CACHE = [
//    '/',
//    '/index.html',
//    '/manifest.webmanifest',
//    '/icon-512.png',
//    '/icon-192.png',
//    // Agrega más archivos según sea necesario
//];

//// Install event - cache files
//self.addEventListener('install', (event) => {
//    event.waitUntil(
//        caches.open(CACHE_NAME)
//            .then((cache) => {
//                return cache.addAll(FILES_TO_CACHE);
//            })
//    );
//});

//// Activate event - clean up old caches
//self.addEventListener('activate', (event) => {
//    event.waitUntil(
//        caches.keys().then((keyList) => {
//            return Promise.all(keyList.map((key) => {
//                if (key !== CACHE_NAME) {
//                    return caches.delete(key);
//                }
//            }));
//        })
//    );
//    self.clients.claim();
//});

//// Fetch event - serve cached content when offline
//self.addEventListener('fetch', (event) => {
//    if (event.request.mode !== 'navigate') {
//        // Not a page navigation, bail.
//        return;
//    }
//    event.respondWith(
//        fetch(event.request)
//            .catch(() => {
//                return caches.open(CACHE_NAME)
//                    .then((cache) => {
//                        return cache.match('index.html');
//                    });
//            })
//    );
//});

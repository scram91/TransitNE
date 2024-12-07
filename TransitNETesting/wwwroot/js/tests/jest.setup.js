// Mock the Google Maps API
global.google = {
  maps: {
    LatLng: jest.fn(),
    Map: jest.fn(),
        DirectionsService: jest.fn().mockImplementation(() => ({
                  route: jest.fn((request, callback) => {
        callback({ routes: [{ legs: [{ distance: { text: '5 km' }, duration: { text: '15 mins' } }], steps: [] }] } , 'OK');
        })
        })),
    DirectionsRenderer: jest.fn(),
    TravelMode: {
      TRANSIT: 'TRANSIT',
      WALKING: 'WALKING'
    },
    DirectionsStatus: {
      OK: 'OK'
    },
    places: {
      Autocomplete: jest.fn()
    }
  }
};

module.exports = {
  setupFiles: ['./jest.setup.js'],
  testEnvironment: 'jsdom'  // Ensure the environment is set to jsdom for DOM-related testing
};
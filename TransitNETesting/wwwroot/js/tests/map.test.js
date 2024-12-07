// Assuming calculateRoute is exported from your map.js file
const { calculateRoute } = require('../../../../TransitNE/wwwroot/js/map.js');

describe('calculateRoute function', () => {
  test('returns expected routes', () => {
    // Mock elements used in the function
    document.body.innerHTML = `
      <input id="startLocation" value="Start Point" />
      <input id="endLocation" value="End Point" />
      <select id="preference">
        <option value="fastest">Fastest Route</option>
        <option value="shortest">Shortest Route</option>
      </select>
    `;

    const mockDirectionsRenderer = { setDirections: jest.fn(), setRouteIndex: jest.fn() };
    
    // Mock the global objects used by the calculateRoute function
    global.directionsService = {
      route: jest.fn((request, callback) => {
        callback({ routes: [{ legs: [{ distance: { text: '5 km' }, duration: { text: '15 mins' } }], steps: [] }] }, 'OK');
      })
    };
    global.directionsRenderer = mockDirectionsRenderer;

    // Call the function
    calculateRoute();

    // Assert that the directionsRenderer.setDirections was called
    expect(mockDirectionsRenderer.setDirections).toHaveBeenCalled();
  });
});
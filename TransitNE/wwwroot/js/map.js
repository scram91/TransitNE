        let geocoder, directionsService, directionsRenderer, transitRoutes;

        function InitializeMap() {
            // Set the Latitude and Longitude of the Map
            var myAddress = new google.maps.LatLng(39.9526, -75.1652);

            // Create Options or set different Characteristics of Google Map
            var mapOptions = {
                center: myAddress,
                zoom: 15,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            // Display the Google map in the div control with the defined Options
            var map = new google.maps.Map(document.getElementById("map"), mapOptions);

            // Initialize DirectionsService and DirectionsRenderer
            directionsService = new google.maps.DirectionsService();
            directionsRenderer = new google.maps.DirectionsRenderer();
            directionsRenderer.setMap(map);

            // Initialize Autocomplete for input fields
              autocompleteStart = new google.maps.places.Autocomplete(document.getElementById("startLocation"));
              autocompleteEnd = new google.maps.places.Autocomplete(document.getElementById("endLocation"));

            // Restrict autocomplete to focus on relevant inputs
            autocompleteStart.setFields(["geometry", "formatted_address"]);
            autocompleteEnd.setFields(["geometry", "formatted_address"]);

        }

        function calculateRoute() {
            const start = document.getElementById("startLocation").value;
            const end = document.getElementById("endLocation").value;
            const preference = document.getElementById("preference").value;

            if (!start || !end) {
                alert("Please enter both start and end locations.");
                return;
            }
            
            directionsService.route( {
                    origin: start, //Start Location
                    destination: end, //End Location
                    travelMode: google.maps.TravelMode.TRANSIT, //Method of travel
                    provideRouteAlternatives: true, //Enable multiple routes
                },
                (result, status) => {
                    console.log("Routes:", result.routes);
                    if (status === google.maps.DirectionsStatus.OK) {
                        transitRoutes = result.routes;

                        let selectedRoute;
                        if (preference === "fastest") {
                            //Use the first route -- Google returns the fastest by default
                            selectedRoute = transitRoutes[0];
                        } else if (preference === "shortest") {
                            //Find the shortest route
                            selectedRoute = transitRoutes.reduce((shortest, current) => {
                                const shortestDistance = shortest.legs[0].distance.value;
                                const currentDistance = current.legs[0].distance.value;
                                return currentDistance < shortestDistance ? current : shortest;
                            });
                        }
                        //Render the Route
                        directionsRenderer.setDirections(result); // Render full result
                        directionsRenderer.setRouteIndex(transitRoutes.indexOf(selectedRoute)); // Ensure the right route

                        // Display route options for user
                        displayRouteSummary(selectedRoute);

                        // Display the route options in the console -- Use for Debugging
                        console.log("Available routes:", result.routes);
                    } else {
                        alert("Could not calculate direction: " + status);
                    }
                }
            );
        }
        
        function displayRouteSummary(route) {
            const summaryDiv = document.getElementById("routeSummary");
            const leg = route.legs[0];

            const stepsSummary = leg.steps
                .map(step => {
                    if (step.travel_mode === "TRANSIT") {
                        const transitDetails = step.transit;
                        const mode = transitDetails.line.vehicle.type || "Unkown Mode";
                        const lineName = transitDetails.line.short_name || transitDetails.line.name || "Unknown Line";
                        return `<strong>Transit</strong>: ${mode}, Line ${lineName}`;
                    } else if (step.travel_mode === "WALKING") {
                        return `<strong>Walking</strong>: ${step.distance.text}, about ${step.duration.text}`;
                    } else {
                        return `<strong>${step.travel_mode}</strong>: ${step.distance.text}, about ${step.duration.text}`;
                    }
                })
                .join("<br>");
            summaryDiv.innerHTML = `
                <strong>Route Details:</strong><br>
                Duration: ${leg.duration.text}<br>
                Distance: ${leg.distance.text}<br>
                Steps:<br>${stepsSummary || "No route steps available."}
            `;
        }
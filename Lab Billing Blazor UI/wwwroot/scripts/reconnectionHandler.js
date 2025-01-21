window.reconnectionHandler = {
    initialize: function () {
        console.log("Reconnection handler initialized.");
        Blazor.start({
            reconnectionHandler: {
                onConnectionDown: function (options, error) {
                    console.log("Connection lost. Attempting to reconnect...");
                    // Notify the user about the connection loss
                },
                onConnectionUp: function (options) {
                    console.log("Connection restored.");
                    // Notify the user about the reconnection
                }
            }
        });
    }
};

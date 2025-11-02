// File download utilities for Blazor
window.fileDownload = {
    downloadFromByteArray: function (byteArray, fileName, contentType) {
        // Convert byte array to blob
        const blob = new Blob([byteArray], { type: contentType });
        const url = window.URL.createObjectURL(blob);
        
        // Create download link
        const link = document.createElement('a');
        link.href = url;
        link.download = fileName;

        // Trigger download
        document.body.appendChild(link);
        link.click();
   
        // Cleanup
        document.body.removeChild(link);
 window.URL.revokeObjectURL(url);
    },

 downloadFromText: function (text, fileName, contentType) {
    // Create blob from text
        const blob = new Blob([text], { type: contentType });
        const url = window.URL.createObjectURL(blob);
        
        // Create download link
        const link = document.createElement('a');
        link.href = url;
        link.download = fileName;
    
   // Trigger download
        document.body.appendChild(link);
        link.click();
        
    // Cleanup
   document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
    },

    downloadFromUrl: function (url, fileName) {
        // Create download link
    const link = document.createElement('a');
        link.href = url;
        link.download = fileName;
        
        // Trigger download
        document.body.appendChild(link);
        link.click();
        
// Cleanup
        document.body.removeChild(link);
    }
};

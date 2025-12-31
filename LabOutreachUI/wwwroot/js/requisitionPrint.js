// Requisition Form Print Helper
// Handles browser-based printing of laboratory requisition forms

window.requisitionPrintHelper = {
    /**
     * Prints HTML content by creating a hidden iframe
     * @param {string} htmlContent - The HTML content to print
     * @param {string} formType - The type of form being printed
     * @returns {Promise<boolean>} - Success status
     */
    printForm: function (htmlContent, formType) {
  return new Promise((resolve, reject) => {
 try {
       // Create a hidden iframe for printing
  let printFrame = document.getElementById('requisition-print-frame');
       
if (!printFrame) {
        printFrame = document.createElement('iframe');
        printFrame.id = 'requisition-print-frame';
       printFrame.style.position = 'absolute';
printFrame.style.width = '0';
         printFrame.style.height = '0';
       printFrame.style.border = 'none';
printFrame.style.visibility = 'hidden';
       document.body.appendChild(printFrame);
          }

      // Wait for iframe to be ready
         printFrame.onload = function () {
         try {
   // Give the browser a moment to render
         setTimeout(() => {
          try {
         printFrame.contentWindow.focus();
       printFrame.contentWindow.print();
      resolve(true);
       } catch (printError) {
        console.error('Print error:', printError);
         reject(printError);
     }
                }, 500);
                } catch (error) {
                     console.error('Frame load error:', error);
               reject(error);
     }
  };

    // Write content to iframe
          const iframeDoc = printFrame.contentDocument || printFrame.contentWindow.document;
       iframeDoc.open();
         iframeDoc.write(htmlContent);
      iframeDoc.close();

            } catch (error) {
       console.error('Setup error:', error);
          reject(error);
       }
        });
    },

    /**
     * Prints a Blazor component element by ID
     * @param {string} elementId - The ID of the element to print
     * @returns {Promise<boolean>} - Success status
*/
    printElement: function (elementId) {
        return new Promise((resolve, reject) => {
            try {
     const element = document.getElementById(elementId);
       
         if (!element) {
    reject(new Error(`Element with ID '${elementId}' not found`));
         return;
            }

     // Get the element's HTML including styles
         const htmlContent = this.buildPrintDocument(element);
                
           // Use the main print function
      this.printForm(htmlContent, 'component')
      .then(resolve)
 .catch(reject);
        
    } catch (error) {
       console.error('Element print error:', error);
    reject(error);
       }
        });
    },

    /**
     * Builds a complete HTML document for printing
     * @param {HTMLElement} element - The element to print
     * @returns {string} - Complete HTML document
     */
    buildPrintDocument: function (element) {
     // Get all stylesheets from the main document
    const stylesheets = Array.from(document.querySelectorAll('link[rel="stylesheet"], style'))
  .map(style => {
          if (style.tagName === 'LINK') {
           return `<link rel="stylesheet" href="${style.href}">`;
                } else {
        return `<style>${style.innerHTML}</style>`;
   }
            })
      .join('\n');

        // Build complete HTML document
        return `
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Requisition Form Print</title>
    ${stylesheets}
    <style>
        @media print {
            body {
     margin: 0;
 padding: 0;
        }
            
         /* Hide everything except form content */
            body > *:not(.form-container) {
       display: none !important;
       }
        
            .form-container {
          margin: 0;
      padding: 0.5in;
      }
        }
        
        @media screen {
    body {
         background: white;
        }
        }
    </style>
</head>
<body>
    ${element.innerHTML}
    <script>
        // Auto-trigger print after page loads
        window.onload = function() {
            // Small delay to ensure rendering is complete
            setTimeout(() => {
                window.print();
            }, 100);
        };
    </script>
</body>
</html>`;
    },

    /**
     * Opens print preview in a new window
     * @param {string} htmlContent - The HTML content to preview
     * @returns {boolean} - Success status
     */
    openPrintPreview: function (htmlContent) {
  try {
       const previewWindow = window.open('', 'PrintPreview', 'width=800,height=600,scrollbars=yes');
        
            if (!previewWindow) {
     alert('Please allow popups for this site to use print preview');
   return false;
}

            previewWindow.document.open();
    previewWindow.document.write(htmlContent);
            previewWindow.document.close();
     
     return true;
        } catch (error) {
       console.error('Preview window error:', error);
            return false;
        }
    },

    /**
     * Cleans up the print iframe
     */
    cleanup: function () {
        const printFrame = document.getElementById('requisition-print-frame');
        if (printFrame) {
            printFrame.remove();
        }
    }
};

// Cleanup on page unload
window.addEventListener('beforeunload', () => {
    window.requisitionPrintHelper.cleanup();
});

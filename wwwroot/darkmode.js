
        
    // Dark Mode aktivieren/deaktivieren und speichern
function toggleDarkMode() {
    // Dark Mode toggeln
    document.body.classList.toggle('dark-mode');
    document.querySelectorAll('.navbar, .footer, .card').forEach(element => {
        element.classList.toggle('dark-mode');
    });

    // Dark Mode Zustand speichern
    const isDarkMode = document.body.classList.contains('dark-mode');
    localStorage.setItem('darkMode', isDarkMode); // Speichern: "true" oder "false"
    const darkModeToggle = document.getElementById('darkModeToggle');
    if (darkModeToggle) {
        darkModeToggle.innerText = isDarkMode ? 'Light Mode' : 'Dark Mode';
    }
}

// Beim Laden der Seite überprüfen
document.addEventListener('DOMContentLoaded', () => {
    const darkMode = localStorage.getItem('darkMode'); // Dark Mode Zustand auslesen
    if (darkMode === 'true') {
        // Dark Mode aktivieren
        document.body.classList.add('dark-mode');
        document.querySelectorAll('.navbar, .footer, .card').forEach(element => {
            element.classList.add('dark-mode');
        });
        const darkModeToggle = document.getElementById('darkModeToggle');
        if (darkModeToggle) {
            darkModeToggle.innerText = 'Light Mode';
        }
    }
});

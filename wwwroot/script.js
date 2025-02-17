function calculatePickupDate(priority) {
    const currentDate = new Date();
    switch (priority) {
        case "tief":
            currentDate.setDate(currentDate.getDate() + 13);
            break;
        case "express":
            currentDate.setDate(currentDate.getDate() + 5);
            break;
        default:
            currentDate.setDate(currentDate.getDate() + 7);
            break;
    }
    return currentDate.toISOString().split('T')[0]; // ISO 8601 Format für SQL
}

document.getElementById("anmeldungForm").addEventListener("submit", async function (event) {
    event.preventDefault(); // Verhindert das Neuladen der Seite

    // Formulardaten erfassen
    const name = document.getElementById("kundenname")?.value.trim();
    const email = document.getElementById("email")?.value.trim();
    const telefon = document.getElementById("telefon")?.value.trim();
    const priority = document.getElementById("prioritaet")?.value;
    const service = document.getElementById("dienstleistung")?.value;
    const pickupDate = calculatePickupDate(priority);
    const createDate = new Date().toISOString().split('T')[0]; // SQL-kompatibles Format

    // Validierung auf leere Felder
    if (!name || !email || !telefon || !priority || !service) {
        alert("Bitte füllen Sie alle Felder aus.");
        return;
    }

    // E-Mail-Validierung
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailPattern.test(email)) {
        alert("Bitte geben Sie eine gültige E-Mail-Adresse ein.");
        return;
    }

    // Telefonnummer-Validierung
    const telefonPattern = /^\+?\d{1,4}[\s-]?\d{1,15}$/;
    if (!telefonPattern.test(telefon)) {
        alert("Bitte geben Sie eine gültige Telefonnummer ein.");
        return;
    }

    // Preisberechnung für den Service
    const priceList = {
        "kleiner-service": 20,
        "grosser-service": 50,
        "rennski-service": 200,
        "bindung": 22,
        "heisswachsen": 60,
        "fell": 50
    };
    const servicePrice = priceList[service] || 0;

    // Daten für die API vorbereiten
    const auftrag = {
        beschreibung: service,
        priorität: priority,
        status: "Neu",
        kunde: {
            name: name,
            email: email,
            telefon: telefon
        },
        create_date: createDate,
        pickup_date: pickupDate
    };

    try {
        const apiUrl = "http://localhost:5180/api/ServiceAuftrag";

        const response = await fetch(apiUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(auftrag)
        });

        if (!response.ok) {
            const errorText = await response.text(); // Fehlertext abrufen
            throw new Error(`Fehler vom Server: ${errorText}`);
        }

        const data = await response.json();
        console.log("Erstellter Auftrag:", data);

        document.getElementById("anmeldungForm").reset(); // Formular zurücksetzen
        showConfirmationPage(name, pickupDate, service, servicePrice); // Bestätigungsseite anzeigen

    } catch (error) {
        console.error("Fehler:", error);
        alert(`Fehler beim Absenden des Formulars: ${error.message}`);
    }
});

// Bestätigungsseite anzeigen
function showConfirmationPage(name, pickupDate, service, servicePrice) {
    document.body.innerHTML = `
        <h1>Vielen Dank für Ihre Anmeldung, ${name}!</h1>
        <p>Ihr gewählter Service: <strong>${service}</strong></p>
        <p>Abholdatum: <strong>${pickupDate}</strong></p>
        <p>Preis: <strong>${servicePrice} CHF</strong></p>
        <button onclick="window.location.href='Index.html'" class="btn btn-primary">Zur Startseite</button>
    `;
}

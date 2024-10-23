

async function loadEmployees() {
    try {
        const response = await fetch('http://localhost:8000/GetAllEmployees');

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const employees = await response.json();
        displayEmployees(employees);
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
    }
}
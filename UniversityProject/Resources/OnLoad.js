// Preload employee data when the home page loads
async function preloadEmployees() {
    try {
        const employeesData = localStorage.getItem('employeesData');
        if (employeesData) {
            console.log('Employee data already preloaded');
            return; // Data already exists, no need to fetch again
        }

        const response = await fetch('http://localhost:8000/GetAllEmployees');
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const employees = await response.json();
        console.log('Employees preloaded:', employees);

        // Store employee data in localStorage for later use
        localStorage.setItem('employeesData', JSON.stringify(employees));

    } catch (error) {
        console.error('Failed to preload employees:', error);
        alert('Failed to load employee data. Please check your connection.');
    }
}

window.addEventListener('load', preloadEmployees);

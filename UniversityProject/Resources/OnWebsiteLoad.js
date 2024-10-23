// Preload employee data when the home page loads
async function preloadEmployees() {
    try {
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
    }
}

window.addEventListener('load', preloadEmployees);

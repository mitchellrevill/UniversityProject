
/*

window.addEventListener('load', preloadEmployees);
window.addEventListener('load', preloadJobPostings);

async function preloadEmployees() {

    try {

        var localEmployeeData = localStorage.getItem('employeesData');

        // Catch to see if data is already preloaded
        if (localEmployeeData) {
            console.log('Employee data already preloaded');
            return;
        }
        else {

            var response = await fetch('http://localhost:8000/GetAllEmployees');

            if (!response.ok) {
                throw new Error('Network response was not ok');
                return;
            }
            else {
                var employees = await response.json();
                console.log('Employees successfully loaded');

                // Store employee data in localStorage for later use
                localStorage.setItem('employeesData', JSON.stringify(employees));
            }
        }

    } catch (error) {
        console.error('Failed to preload employees:', error);
        alert('Failed to load employee data. Please check your connection.');
    }
}




async function preloadJobPostings() {
    try {
        var localJobPostingData = localStorage.getItem('jobPostingData');

        // Check if data is already preloaded
        if (localJobPostingData) {
            console.log('Job postings data already preloaded');
            return;
        } else {
            var response = await fetch('http://localhost:8000/GetAllJobPostings');

            if (!response.ok) {
                throw new Error('Network response was not ok');
            } else {
                var jobPostings = await response.json();
                console.log('Job postings loaded:');

                // Store job postings data in localStorage for later use
                localStorage.setItem('jobPostingData', JSON.stringify(jobPostings));
            }
        }
    } catch (error) {
        console.error('Failed to preload job postings:', error);
        alert('Failed to load job postings data. Please check your connection.');
    }
}




*/
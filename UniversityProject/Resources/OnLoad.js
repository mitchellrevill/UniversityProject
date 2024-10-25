/*
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

// Preload employee data when the home page loads
async function preloadJobPostings() {
    try {
a  
        const JobPostingData = localStorage.getItem('JobPostingData');
        if (JobPostingData) {
            console.log('JobPosting Data data already preloaded');
            return; // Data already exists, no need to fetch again
        }

        const response = await fetch('http://localhost:8000/GetAllJobPostings;
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const employees = await response.json();
        console.log('Job postings preloaded:', JobPostings);

        // Store employee data in localStorage for later use
        localStorage.setItem('JobPostingData', JSON.stringify(JobPostings));

    } catch (error) {
        console.error('Failed to preload Job Postings:', error);
        alert('Failed to load job postings data. Please check your connection.');
    }
}

window.addEventListener('load', preloadJobPostings);
*/
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

// Preload job postings when the home page loads
async function preloadJobPostings() {
    try {
        const jobPostingData = localStorage.getItem('jobPostingData');
        if (jobPostingData) {
            console.log('Job postings data already preloaded');
            return; // Data already exists, no need to fetch again
        }

        const response = await fetch('http://localhost:8000/GetAllJobPostings');
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const jobPostings = await response.json();
        console.log('Job postings preloaded:', jobPostings);

        // Store job postings data in localStorage for later use
        localStorage.setItem('jobPostingData', JSON.stringify(jobPostings));

    } catch (error) {
        console.error('Failed to preload job postings:', error);
        alert('Failed to load job postings data. Please check your connection.');
    }
}

window.addEventListener('load', preloadJobPostings);

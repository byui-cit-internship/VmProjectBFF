pipeline {
    
        agent any
        
        
    
        stages {

            stage("Clean workspace") 
            {
                steps 
                {
                    echo ' Cleaning workspace........'
                    cleanWs()
                }

                // steps
                // {
                //     cleanWs()
                // } 
            }
            stage("Git Checkout")
            {
                steps
                {
                    echo 'Checking out ......'
                    checkout([$class: 'GitSCM', branches: [[name: '*/main']], extensions: [], userRemoteConfigs: [[credentialsId: '7bcf443d-e5d2-4fe3-ae4a-eebb83b6df91', url: 'git@github.com:BYUI-CIT-Internship/VmProject.git']]])
                }
            }
            stage("Checkout") 
            {
                steps 
                {
                    echo 'Checking out ........'

                }   
            }
            stage("Build") 
            {
                steps
                {
                    echo 'Building the application ........'
                }
                
            }

            stage("Test") 
            {
                steps
                {
                    echo 'testing the application ........'
                }
            }
            stage("Deploying") 
            {
                steps
                {
                    echo 'Deploying the application to production ........'
                }
            }

        }
        
}
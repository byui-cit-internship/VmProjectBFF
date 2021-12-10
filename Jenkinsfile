pipeline {
    environment {
        imagename = "gcr.io/constellation-275522/vmproject:latest"
        registryCredential = 'CIT Department Infrastructure'
        dockerImage = ''
    }
    agent any
    stages {
        stage('Cloning Git') {
            steps {
                git([url: 'git@github.com:BYUI-CIT-Internship/VmProject.git', branch: 'main', credentialsId: 'Github_VMAPI_DeployKey'])
            }
        }
        stage('Building image') {
            steps{
                script {
                    dockerImage = docker.build(imagename, './vmProjectBackend')
                }
            }
        }
        stage('Deploy Image') {
            steps{
                script {
                    docker.withRegistry( 'https://gcr.io/constellation-275522/vmproject', "gcr:gke" ) {
                        dockerImage.push("$BUILD_NUMBER")
                        dockerImage.push('latest')
                    }
                }
            }
        }
        stage('Remove Unused docker image') {
            steps{
                sh "docker rmi $imagename"
                //sh "docker rmi vmproject:latest"
            }
        }
    }
}
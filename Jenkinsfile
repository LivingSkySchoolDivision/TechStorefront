pipeline {
    agent any
    environment {
        REPO = "techstorefront/storefront"
        PRIVATE_REPO = "${PRIVATE_DOCKER_REGISTRY}/${REPO}"
        PRIVATE_REPO_MAN = "${PRIVATE_DOCKER_REGISTRY}/${REPO}-manager"
        PRIVATE_REPO_EMR = "${PRIVATE_DOCKER_REGISTRY}/${REPO}-emailrunner"
        TAG = "j-${env.BUILD_NUMBER}"
    }
    stages {
        stage('Test') {
            agent {
                docker { 
                    image 'mcr.microsoft.com/dotnet/core/sdk:2.2-stretch'
                    args '-v ${PWD}:/app'
                }
            }
            steps {
                git branch: 'master',
                    credentialsId: 'JENKINS-AZUREDEVOPS',
                    url: 'git@ssh.dev.azure.com:v3/LivingSkySchoolDivision/LSSDStoreFront/LSSDStoreFront'
               
                dir("LSSDStoreFront") {
                    sh 'dotnet build'
                    sh 'dotnet test'
                }
            }
        }
        stage('Docker build') {            
            steps {
                git branch: 'master',
                    credentialsId: 'JENKINS-AZUREDEVOPS',
                    url: 'git@ssh.dev.azure.com:v3/LivingSkySchoolDivision/LSSDStoreFront/LSSDStoreFront'

                dir("LSSDStoreFront") {
                    sh "docker build -t ${PRIVATE_REPO}:latest -f Dockerfile-Frontend -t ${PRIVATE_REPO}:${TAG} ."
                    sh "docker build -t ${PRIVATE_REPO_MAN}:latest -f Dockerfile-Manager -t ${PRIVATE_REPO_MAN}:${TAG} ." 
                    sh "docker build -t ${PRIVATE_REPO_EMR}:latest -f Dockerfile-EmailRunner -t ${PRIVATE_REPO_EMR}:${TAG} ."                        
                }                           
            }
        }
        stage('Docker push') {
            steps {
                sh "docker push ${PRIVATE_REPO}:${TAG}"
                sh "docker push ${PRIVATE_REPO}:latest" 
                sh "docker push ${PRIVATE_REPO_MAN}:${TAG}"
                sh "docker push ${PRIVATE_REPO_MAN}:latest"   
                sh "docker push ${PRIVATE_REPO_EMR}:${TAG}"
                sh "docker push ${PRIVATE_REPO_EMR}:latest"           
            }
        }
    }
    post {        
        always {              
            deleteDir()
            sh "docker image rm ${PRIVATE_REPO}:${TAG}"
            sh "docker image rm ${PRIVATE_REPO}:latest" 
            sh "docker image rm ${PRIVATE_REPO_MAN}:${TAG}"
            sh "docker image rm ${PRIVATE_REPO_MAN}:latest"  
            sh "docker image rm ${PRIVATE_REPO_EMR}:${TAG}"
            sh "docker image rm ${PRIVATE_REPO_EMR}:latest"  
            deleteDir()
        }
        failure {
            mail to:'jenkinsalerts@lskysd.ca',
                subject: "Failed Pipeline: ${currentBuild.fullDisplayName}",
                body: "Something is wrong with ${env.BUILD_URL}"
        }
    }
}
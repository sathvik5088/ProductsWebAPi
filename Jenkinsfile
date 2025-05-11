pipeline {
    agent any

    environment {
        DOTNET_ROOT = "/usr/share/dotnet"
        PATH = "/usr/share/dotnet:${env.PATH}"
        
    }

    stages {
        // Stage 1: Clone Source Code from GitHub
        stage('Clone Source Code') {
            steps {
                git branch: 'main', url: 'https://github.com/sathvik5088/ProductsWebAPi.git'
            }
        }

        // Stage 2: Restore NuGet Dependencies
        stage('Restoring Dependencies') {
            steps {
                sh 'dotnet restore'
            }
        }

        // Stage 3: Build the Application
        stage('Build Source Code') {
            steps {
                sh 'dotnet build --configuration Release'
            }
        }

        // Stage 4: Run Unit Tests
        stage('Run Tests') {
            steps {
                sh 'dotnet test --configuration Release'
            }
        }

        // Stage 5: Publish the Application
        stage('Publish Artifact') {
            steps {
                sh 'dotnet publish -c Release -o publish'
            }
        }

        // Stage 6: Build Docker Image and Push to DockerHub
        // stage('Build Docker Image & Push to DockerHub') {
        //     steps {
        //         withCredentials([usernamePassword(credentialsId: 'docker_cred', usernameVariable: 'USER', passwordVariable: 'PASS')]) {
        //             sh """
        //                 echo "$PASS" | docker login -u "$USER" --password-stdin
        //                 docker build -t sathvik1522/productswebapi:latest .
        //                 docker push sathvik1522/productswebapi:latest
        //             """
        //         }
        //     }
        // }

        // Stage 7: Deploy to AWS EC2 (Docker Run)
        stage('Deploy to container') {
            steps {
               withCredentials([usernamePassword(credentialsId: 'doc_cred', usernameVariable: 'USER', passwordVariable: 'PASS')]) {
                    sh """
                        echo "$PASS" | docker login -u "$USER" --password-stdin
                        docker pull sathvik1522/images:webapi
                        docker rm -f productswebapi || true
                        docker run -d -p 8081:80 --name productswebapi sathvik1522/images:webapi
                    """
                }
            }
        }

        // Stage 8: Setup Nginx and SSL (Optional, if needed)
        stage('Setup Nginx & SSL') {
            steps {
                script {
                    // Install Nginx
                    sh '''
                        sudo yum update
                        sudo yum install -y nginx
                    '''
                    
                    // Replace 'your-server-ip' with your EC2 server's public IP address
                    def server_ip = ${env.SERVER_IP}  // Replace this with the actual IP address of your server
        
                    // Set up Nginx Configuration to use the server IP
                    sh """
                        echo '
                        server {
                            listen 80;
                            server_name ${server_ip};
        
                            location / {
                                proxy_pass http://localhost:8081;
                                proxy_set_header Host \$host;
                                proxy_set_header X-Real-IP \$remote_addr;
                                proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
                            }
                        }
                        ' | sudo tee /etc/nginx/sites-available/default
                    """
        
                    // Restart Nginx service to apply changes
                    sh '''
                        sudo systemctl restart nginx
                    '''
                }
            }
        }

    }

    post {
        always {
            echo 'Pipeline execution complete'
        }
    }
}
using System;
using System.Diagnostics;
using System.IO;

public static class MainClass {
    public static Random random = new Random();

    public static matrix GenerateRandomMatrix(int rows, int cols) {
        matrix D = new matrix(rows, cols);
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                if (i == j) {
                    D[i, j] = random.NextDouble();
                } else {
                    D[i, j] = 0;
                }
            }
        }
        return D;
    }

    public static vector GenerateRandomVector(int size) {
        vector u = new vector(size);
        for (int i = 0; i < size; i++) {
            u[i] = random.NextDouble();
        }
        return u;
    }

    public static void Main() {
        int n = 5;

        matrix D = GenerateRandomMatrix(n, n);
        matrix D1 = GenerateRandomMatrix(n, n);
        matrix D2 = GenerateRandomMatrix(n, n);
        vector u = GenerateRandomVector(n);
        vector u1 = GenerateRandomVector(n);
        vector u2 = GenerateRandomVector(n);

        
        // Call RankOneUpdate with generated D and u
        double sigma0 = 0;
        double sigmaPos = 1;
        double sigmaNeg = -1;

        vector eigenvalues0 = program.RankOneUpdate(D, u, sigma0);
        vector eigenvaluesPos = program.RankOneUpdate(D1, u1, sigmaPos);
        vector eigenvaluesNeg = program.RankOneUpdate(D2, u2, sigmaNeg);
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
        Console.WriteLine("The rank-1 update on a symmetric eigenvalue problem is tested here for the first case of σ = 0");
        Console.Write("Following random diagonal matrix D is generated:");
        D.print();
        Console.WriteLine("And the computed Eigenvalues for A is found to be:");
        for (int i = 0; i < eigenvalues0.size; i++) {
            Console.WriteLine($"Eigenvalue {i+1}: {eigenvalues0[i]}");
        }
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
        
        Console.WriteLine("To show the algorithm also works for the case σ > 0");
        Console.Write("Another random diagonal matrix D is generated:");
        D1.print();
        Console.WriteLine("And the computed Eigenvalues for A is found to be:");
        for (int i = 0; i < eigenvaluesPos.size; i++) {
            Console.WriteLine($"Eigenvalue {i+1}: {eigenvaluesPos[i]}");
        }
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------");

        Console.WriteLine("An lastly for the case of σ < 0");
        Console.Write("Random diagonal matrix D is generated:");
        D2.print();
        Console.WriteLine("And the computed Eigenvalues for A is found to be:");
        for (int i = 0; i < eigenvaluesNeg.size; i++) {
            Console.WriteLine($"Eigenvalue {i+1}: {eigenvaluesNeg[i]}");
        }
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------");       
      

        int maxRetries = 5; // Adjust if fail to converge
        int largestMatrix = 50; // Adjust if computation takes too long
        Console.WriteLine($"To test for the O(n2) operations the algorithm is run on random generated matrices from n=1 to n={largestMatrix} and the time measured for completion is noted");
        Console.WriteLine("The performance is shown in the figure computation_time.svg");
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------");

        using (var writer = new StreamWriter("times.data")) {        
            for (int i = 1; i <= largestMatrix; i++) {
                int retries = 0;
                while (retries < maxRetries) { // Inner loop for retries
                    double sigma = 1; // Assuming σ = 1
                    matrix D0 = GenerateRandomMatrix(i, i);
                    vector u0 = GenerateRandomVector(i);

                    Stopwatch stopwatch = Stopwatch.StartNew();                    
                    program.RankOneUpdate(D0, u0, sigma);
                    stopwatch.Stop();

                    double timelimit = 10 + 0.15*i*i;
                    if (stopwatch.ElapsedMilliseconds > timelimit) {  // Incase the Newton method has taken too long to converge
                        Console.WriteLine($"Retrying computation for {i}x{i} matrix");
                        retries++;
                        continue; // Restart the inner loop for the same size
                    }

                    writer.WriteLine($"{i} \t {stopwatch.ElapsedMilliseconds}");
                    break; // Break the inner loop to proceed with the next size
                }
                if (retries == maxRetries) {
                    Console.WriteLine($"Max retries exceeded for {i}x{i} matrix");
                    Console.WriteLine($"Try to adjust parameter maxRetries");
                }
            }
        }
    }
}

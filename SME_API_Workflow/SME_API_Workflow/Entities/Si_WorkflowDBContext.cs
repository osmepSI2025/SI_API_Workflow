using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SME_API_Workflow.Entities;

public partial class Si_WorkflowDBContext : DbContext
{
    public Si_WorkflowDBContext()
    {
    }

    public Si_WorkflowDBContext(DbContextOptions<Si_WorkflowDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MApiInformation> MApiInformations { get; set; }

    public virtual DbSet<MScheduledJob> MScheduledJobs { get; set; }

    public virtual DbSet<MWorkflow> MWorkflows { get; set; }

    public virtual DbSet<MWorkflowActivity> MWorkflowActivities { get; set; }

    public virtual DbSet<MWorkflowControlPoint> MWorkflowControlPoints { get; set; }

    public virtual DbSet<MWorkflowLeadingLagging> MWorkflowLeadingLaggings { get; set; }

    public virtual DbSet<TWorkflowActivity> TWorkflowActivitys { get; set; }

    public virtual DbSet<TWorkflowControlPointActivityDetail> TWorkflowControlPointActivityDetails { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=27.254.173.62;Database=bluecarg_SME_API_WorkFlow;User Id=SME_WorkFlow;Password=4xt$Fv812;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("SME_WorkFlow");

        modelBuilder.Entity<MApiInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MApiInformation");

            entity.ToTable("M_ApiInformation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApiKey).HasMaxLength(150);
            entity.Property(e => e.AuthorizationType).HasMaxLength(50);
            entity.Property(e => e.Bearer).HasColumnType("ntext");
            entity.Property(e => e.ContentType).HasMaxLength(150);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.MethodType).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(150);
            entity.Property(e => e.ServiceNameCode).HasMaxLength(250);
            entity.Property(e => e.ServiceNameTh).HasMaxLength(250);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Urldevelopment).HasColumnName("URLDevelopment");
            entity.Property(e => e.Urlproduction).HasColumnName("URLProduction");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<MScheduledJob>(entity =>
        {
            entity.ToTable("M_ScheduledJobs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.JobName).HasMaxLength(150);
        });

        modelBuilder.Entity<MWorkflow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__M_Workfl__3213E83F79507561");

            entity.ToTable("M_Workflow");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateWorkflow)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("createWorkflow");
            entity.Property(e => e.HaveDigital)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("haveDigital");
            entity.Property(e => e.HaveWorkflow)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("haveWorkflow");
            entity.Property(e => e.PerformanceIndicator)
                .HasMaxLength(255)
                .HasColumnName("performanceIndicator");
            entity.Property(e => e.Period)
                .HasMaxLength(100)
                .HasColumnName("period");
            entity.Property(e => e.Urls)
                .HasMaxLength(500)
                .HasColumnName("URLs");
            entity.Property(e => e.WorkflowCode)
                .HasMaxLength(50)
                .HasColumnName("workflowCode");
            entity.Property(e => e.WorkflowGroupCode)
                .HasMaxLength(50)
                .HasColumnName("workflowGroupCode");
            entity.Property(e => e.WorkflowName)
                .HasMaxLength(255)
                .HasColumnName("workflowName");
            entity.Property(e => e.WorkflowType)
                .HasMaxLength(100)
                .HasColumnName("workflowType");
        });

        modelBuilder.Entity<MWorkflowActivity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__M_Workfl__3213E83FF9AEAF63");

            entity.ToTable("M_WorkflowActivity");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Period)
                .HasMaxLength(100)
                .HasColumnName("period");
            entity.Property(e => e.WorkflowCode)
                .HasMaxLength(50)
                .HasColumnName("workflowCode");
            entity.Property(e => e.WorkflowGroupCode)
                .HasMaxLength(50)
                .HasColumnName("workflowGroupCode");
            entity.Property(e => e.WorkflowName)
                .HasMaxLength(255)
                .HasColumnName("workflowName");
            entity.Property(e => e.WorkflowType)
                .HasMaxLength(100)
                .HasColumnName("workflowType");
        });

        modelBuilder.Entity<MWorkflowControlPoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__M_Workfl__3213E83F9DE34D83");

            entity.ToTable("M_WorkflowControlPoint");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Period)
                .HasMaxLength(100)
                .HasColumnName("period");
            entity.Property(e => e.WorkflowCode)
                .HasMaxLength(50)
                .HasColumnName("workflowCode");
            entity.Property(e => e.WorkflowGroupCode)
                .HasMaxLength(50)
                .HasColumnName("workflowGroupCode");
            entity.Property(e => e.WorkflowName)
                .HasMaxLength(255)
                .HasColumnName("workflowName");
            entity.Property(e => e.WorkflowType)
                .HasMaxLength(100)
                .HasColumnName("workflowType");
        });

        modelBuilder.Entity<MWorkflowLeadingLagging>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__M_Workfl__3213E83F9ACBC7F3");

            entity.ToTable("M_WorkflowLeadingLagging");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Period)
                .HasMaxLength(100)
                .HasColumnName("period");
            entity.Property(e => e.WorkflowCode)
                .HasMaxLength(50)
                .HasColumnName("workflowCode");
            entity.Property(e => e.WorkflowGroupCode)
                .HasMaxLength(50)
                .HasColumnName("workflowGroupCode");
            entity.Property(e => e.WorkflowName)
                .HasMaxLength(255)
                .HasColumnName("workflowName");
            entity.Property(e => e.WorkflowType)
                .HasMaxLength(100)
                .HasColumnName("workflowType");
        });

        modelBuilder.Entity<TWorkflowActivity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__T_Workfl__3213E83F872E9444");

            entity.ToTable("T_WorkflowActivitys");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activity)
                .HasMaxLength(255)
                .HasColumnName("activity");
            entity.Property(e => e.ControlPoint)
                .HasMaxLength(100)
                .HasColumnName("controlPoint");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.WorkflowId).HasColumnName("workflowId");

            entity.HasOne(d => d.Workflow).WithMany(p => p.TWorkflowActivities)
                .HasForeignKey(d => d.WorkflowId)
                .HasConstraintName("FK__T_Workflo__workf__35BCFE0A");
        });

        modelBuilder.Entity<TWorkflowControlPointActivityDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__T_Workfl__3213E83F3EA05BEA");

            entity.ToTable("T_WorkflowControlPointActivityDetails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activity)
                .HasMaxLength(255)
                .HasColumnName("activity");
            entity.Property(e => e.ControlPoint)
                .HasMaxLength(100)
                .HasColumnName("controlPoint");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.WorkflowId).HasColumnName("workflowId");

            entity.HasOne(d => d.Workflow).WithMany(p => p.TWorkflowControlPointActivityDetails)
                .HasForeignKey(d => d.WorkflowId)
                .HasConstraintName("FK__T_Workflo__workf__3A81B327");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

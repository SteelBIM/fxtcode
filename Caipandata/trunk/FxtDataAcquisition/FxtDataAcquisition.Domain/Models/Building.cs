namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Building
    {
        public int BuildingId { get; set; }
        public Guid? AppId { get; set; }
        /// <summary>
        /// ¥������
        /// </summary>
        public string BuildingName { get; set; }

        public int ProjectId { get; set; }
        /// <summary>
        /// ¥����;
        /// </summary>
        public int? PurposeCode { get; set; }
        /// <summary>
        /// �����ṹ
        /// </summary>
        public int? StructureCode { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public int? BuildingTypeCode { get; set; }
        /// <summary>
        /// �ܲ���
        /// </summary>
        public int? TotalFloor { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        public decimal? FloorHigh { get; set; }
        /// <summary>
        /// �������֤
        /// </summary>
        public string SaleLicence { get; set; }
        /// <summary>
        /// �ݻ���
        /// </summary>
        public string ElevatorRate { get; set; }
        /// <summary>
        /// ��Ԫ��
        /// </summary>
        public int? UnitsNumber { get; set; }
        /// <summary>
        /// �ܻ���
        /// </summary>
        public int? TotalNumber { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        public decimal? TotalBuildArea { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime? BuildDate { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime? SaleDate { get; set; }
        /// <summary>
        /// ¥������
        /// </summary>
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// ���۲�
        /// </summary>
        public int? AverageFloor { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        public DateTime? JoinDate { get; set; }
        /// <summary>
        /// Ԥ��ʱ��
        /// </summary>
        public DateTime? LicenceDate { get; set; }
        /// <summary>
        /// ¥������
        /// </summary>
        public string OtherName { get; set; }
        /// <summary>
        /// �۸�ϵ��
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// �Ƿ�ɹ�
        /// </summary>
        public int? IsEValue { get; set; }

        public int CityID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Valid { get; set; }
        /// <summary>
        /// ���۾���
        /// </summary>
        public decimal? SalePrice { get; set; }

        public DateTime? SaveDateTime { get; set; }

        public string SaveUser { get; set; }
        /// <summary>
        /// λ��
        /// </summary>
        public int? LocationCode { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public int? SightCode { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public int? FrontCode { get; set; }

        public int? FxtCompanyId { get; set; }

        public decimal? X { get; set; }

        public decimal? Y { get; set; }

        public int? XYScale { get; set; }
        /// <summary>
        /// ��ǽװ��
        /// </summary>
        public int? Wall { get; set; }
        /// <summary>
        /// �Ƿ������
        /// </summary>
        public int? IsElevator { get; set; }
        /// <summary>
        /// �������ݾ���
        /// </summary>
        public decimal? SubAveragePrice { get; set; }
        /// <summary>
        /// �۸�ϵ��˵��
        /// </summary>
        public string PriceDetail { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        public int? BHouseTypeCode { get; set; }
        /// <summary>
        /// ¥���
        /// </summary>
        public int? Distance { get; set; }
        /// <summary>
        /// ���²���
        /// </summary>
        public int? Basement { get; set; }
        /// <summary>
        /// ���ƺţ���ַ��
        /// </summary>
        public string Doorplate { get; set; }
        /// <summary>
        /// ��Ȩ��ʽ
        /// </summary>
        public int? RightCode { get; set; }
        /// <summary>
        /// �Ƿ�����¥��
        /// </summary>
        public int? IsVirtual { get; set; }
        /// <summary>
        /// ¥��ֲ�
        /// </summary>
        public string FloorSpread { get; set; }
        /// <summary>
        /// ȹ¥����
        /// </summary>
        public int? PodiumBuildingFloor { get; set; }
        /// <summary>
        /// ȹ¥���
        /// </summary>
        public decimal? PodiumBuildingArea { get; set; }
        /// <summary>
        /// ��¥���
        /// </summary>
        public decimal? TowerBuildingArea { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        public decimal? BasementArea { get; set; }
        /// <summary>
        /// ��������;
        /// </summary>
        public string BasementPurpose { get; set; }
        /// <summary>
        /// סլ����
        /// </summary>
        public int? HouseNumber { get; set; }
        /// <summary>
        /// סլ�����
        /// </summary>
        public decimal? HouseArea { get; set; }
        /// <summary>
        /// ��סլ����
        /// </summary>
        public int? OtherNumber { get; set; }
        /// <summary>
        /// ��סլ���
        /// </summary>
        public decimal? OtherArea { get; set; }
        /// <summary>
        /// �ڲ�װ��
        /// </summary>
        public int? InnerFitmentCode { get; set; }
        /// <summary>
        /// ���㻧��
        /// </summary>
        public int? FloorHouseNumber { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public int? LiftNumber { get; set; }
        /// <summary>
        /// ����Ʒ��
        /// </summary>
        public string LiftBrand { get; set; }
        /// <summary>
        /// �豸��ʩ
        /// </summary>
        public string Facilities { get; set; }
        /// <summary>
        /// �ܵ�ȼ��
        /// </summary>
        public int? PipelineGasCode { get; set; }
        /// <summary>
        /// ��ů��ʽ
        /// </summary>
        public int? HeatingModeCode { get; set; }
        /// <summary>
        /// ǽ������
        /// </summary>
        public int? WallTypeCode { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// �Ƿ��Ժ��
        /// </summary>
        public int? IsYard { get; set; }
        public int? Status { get; set; }
        /// <summary>
        /// ά�����
        /// </summary>
        public int? MaintenanceCode { get; set; }

        public int? FxtBuildingId { get; set; }

        public string Creator { get; set; }

        public int? TempletId { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<House> Houses { get; set; }
        public virtual ICollection<HouseDetails> HouseDetails { get; set; }
        public virtual Templet Templet { get; set; }
    }
}

<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="text" indent="yes"/>

  <xsl:template match="Interchange">
    <xsl:apply-templates select="FunctionGroup/Transaction/Loop[@LoopId='2000']/Loop[@LoopId='2100']"/>
  </xsl:template>

  <xsl:template match="Loop[@LoopId='2100']" >
    <xsl:variable name="trans" select="../../."/>
    <xsl:variable name="payer" select="../../Loop[@LoopId='1000A']"/>
    <xsl:variable name="payee" select="../../Loop[@LoopId='1000B']"/>
    <xsl:variable name="payment" select="."/>
    <xsl:value-of select="$trans/BPR/BPR16"/>
    <xsl:value-of select="','"/>
    <xsl:value-of select="$payer/N1/N102"/>
    <xsl:value-of select="','"/>
    <xsl:value-of select="$payee/N1/N102"/>
    <xsl:value-of select="','"/>
    <xsl:value-of select="$payee/N1/N104"/>
    <xsl:value-of select="','"/>
    <xsl:value-of select="$payment/CLP/CLP01"/>
    <xsl:value-of select="','"/>
    <xsl:value-of select="$payment/CLP/CLP02"/>
    <xsl:value-of select="','"/>
    <xsl:value-of select="$payment/CLP/CLP03"/>
    <xsl:value-of select="','"/>
    <xsl:value-of select="$payment/NM1[NM101='QC']/NM103"/>
    <xsl:value-of select="','"/>
    <xsl:value-of select="$payment/NM1[NM101='QC']/NM104"/>
    <xsl:value-of select="','"/>
    <xsl:value-of select="$payment/CAS[CAS01='CO']/CAS02"/>
    <xsl:value-of select="','"/>
    <xsl:value-of select="$payment/CAS[CAS01='CR']/CAS02"/>
    <xsl:value-of select="','"/>
    <xsl:value-of select="$payment/CAS[CAS01='OA']/CAS02"/>
    <xsl:value-of select="','"/>
    <xsl:value-of select="$payment/CAS[CAS01='PI']/CAS02"/>
    <xsl:value-of select="','"/>
    <xsl:value-of select="$payment/CAS[CAS01='PR']/CAS02"/>
    <xsl:value-of select="','"/>
    <xsl:text>&#x0A;</xsl:text>
  </xsl:template>
</xsl:stylesheet>